using App.Data.Contexts;
using App.Data.Entities.Auth;
using App.Shared.Dto.Auth;
using App.Shared.Security;
using App.Shared.Services.File;
using App.Shared.Services.Mail;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace App.Auth.Api.Services;

public class AuthService(AuthDbContext authDbContext, TokenHelper tokenHelper, IMailService emailService) : IAuthService
{
    public async Task<Result> ForgotPasswordAsync(ForgotPasswordRequest forgotPasswordRequest)
    {
        var user = await authDbContext.Users.SingleOrDefaultAsync(x => x.Email == forgotPasswordRequest.Email);
        if (user is null)
            return Result.Error("User not found");

        var guid = Guid.NewGuid().ToString().Substring(0, 8);

        byte[] passwordHash, passwordSalt;
        HashingHelper.CreatePasswordHash(guid, out passwordHash, out passwordSalt);

        user.PasswordSalt = passwordSalt;
        user.PasswordHash = passwordHash;

        authDbContext.Users.Update(user);
        await authDbContext.SaveChangesAsync();

        var mailMessage = "<p>Tihs your new passford. Plesa don't share your password with anyone!</p>" +
                         $"<h1><b>{guid}</b></h1>";
        await emailService.SendEmailAsync(user.Email, "Reset Password", mailMessage);

        return Result.Success();
    }

    public async Task<Result<LoggedResponse>> LoginAsync(LoginDto loginDto)
    {
        var user = await authDbContext.Users
            .Include(x => x.Role)
            .Include(x => x.RefreshTokens)
            .SingleOrDefaultAsync(x => x.Email == loginDto.Email);

        if (user is null)
            return Result<LoggedResponse>.NotFound("User not found");

        var passwordValid = HashingHelper.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt);
        if (!passwordValid)
            return Result<LoggedResponse>.Invalid(new ValidationError("Invalid password"));

        if (!user.IsActive)
        {
            await SendVerifyEmail(authDbContext, emailService, user);
            return Result<LoggedResponse>.Error("Please verify your email");
        }

        await tokenHelper.RevokedOldRefreshTokens(user); //Logout olunca RefreshToken bir daha kullanılmasın diye.

        var token = tokenHelper.CreateAccessToken(user);
        var refreshToken = await tokenHelper.CreateRefreshTokenAsync(user);

        return Result<LoggedResponse>.Success(new LoggedResponse
        {
            AccessToken = token,
            RefreshToken = refreshToken.Token
        });
    }

    public async Task<Result> RegisterAsync(RegisterDto registerDto)
    {
        var email = await authDbContext.Users.SingleOrDefaultAsync(x => x.Email == registerDto.Email);
        if (email is not null)
            return Result.Error("Email already exists");

        HashingHelper.CreatePasswordHash(registerDto.Password, out var passwordHash, out var passwordSalt);
        var user = new User
        {
            Email = registerDto.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            UserName = registerDto.UserName,
            RoleId = 2
        };

        await authDbContext.Users.AddAsync(user);
        await authDbContext.SaveChangesAsync();
        await SendVerifyEmail(authDbContext, emailService, user);

        return Result.Success();
    }


    public async Task<Result> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest)
    {
        var user = await authDbContext.Users.SingleOrDefaultAsync(x => x.Email == resetPasswordRequest.Email);
        if (user is null)
            return Result.Error("User not found");

        if(resetPasswordRequest.Password != resetPasswordRequest.PasswordRepeat)
            return Result.Error("Passwords do not match");

        HashingHelper.CreatePasswordHash(resetPasswordRequest.Password, out var passwordHash, out var passwordSalt);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        authDbContext.Users.Update(user);
        await authDbContext.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> VerifyEmailAsync(VerifyEmailDto verifyEmailDto)
    {
       var user = await authDbContext.Users.SingleOrDefaultAsync(x => x.Email == verifyEmailDto.Email);
        if (user is null)
            return Result.Error("User not found");

        if (user.VerificationCode != verifyEmailDto.Code)
            return Result.Error("Invalid verification code");

        user.IsActive = true;
        await authDbContext.SaveChangesAsync();

        return Result.Success();

    }
    private static async Task SendVerifyEmail(AuthDbContext authDbContext, IMailService emailService, User user)
    {
        var verificationCode = Guid.NewGuid().ToString().Substring(0, 6);
        user.VerificationCode = verificationCode;
        await authDbContext.SaveChangesAsync();

        string domain = "https://localhost:7213";
        string httpEncodeEmail = HttpUtility.UrlEncode(user.Email);
        string verificationLink = $"{domain}/api/auth/verify-email?Email={httpEncodeEmail}&Code={verificationCode}";

        var emailBody = $"""
                            <h1>Welcome{user.UserName}</h1>
                            <p>Your verification code is: {verificationCode}</p>
                            <p>Click the link below to verify your email</p>
                            <a href='{verificationLink}'>Verify Email</a>
                            """;
        await emailService.SendEmailAsync(user.Email, "Welcome to MessagingApp!", emailBody);
    }

    public async Task<Result<RefreshedTokenResponse>> RefrehsTokenAsync(RefreshTokenRequest refreshTokenRequest)
    {
        var refreshToken = await authDbContext.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshTokenRequest.Token);

        if (refreshToken is null)
            return Result.Error("Refresh token not found");

        if (refreshToken.Revoked is not null)
            return Result.Error("Refresh token revoked");

        if (refreshToken.ExpiresAt < DateTime.Now)
            return Result.Error("Refresh token expired");

        var user = await authDbContext.Users.SingleOrDefaultAsync(x => x.Id == refreshToken.UserId);
        if (user is null)
            return Result.Error("User not found");

        var newRefreshToken = await tokenHelper.RotateRefreshToken(user, refreshToken);
        var newAccessToken = tokenHelper.CreateAccessToken(user);

        return Result<RefreshedTokenResponse>.Success(new RefreshedTokenResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken.Token
        });
    }
    }
