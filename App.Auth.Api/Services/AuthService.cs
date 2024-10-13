using App.Data.Contexts;
using App.Data.Entities.Auth;
using App.Shared.Dto.Auth;
using App.Shared.Security;
using App.Shared.Services.Abstract;
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

        string domain = "https://localhost:7213";
        string httpEncodeEmail = HttpUtility.UrlEncode(user.Email);
        var resetPasswordLink = $"{domain}/api/auth/reset-password?Email={httpEncodeEmail}";

        var emailBody = $@"<h1>Reset Password</h1>
                            <p>Click the link below to reset your password</p>
                            <a href='{resetPasswordLink}'>Reset Password</a>";
        await emailService.SendEmailAsync(user.Email, "Reset Password", emailBody);

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
}
