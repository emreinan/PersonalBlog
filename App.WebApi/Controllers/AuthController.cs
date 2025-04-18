using App.Data.Contexts;
using App.Data.Entities;
using App.Shared.Dto.Auth;
using App.Shared.Security;
using App.Shared.Services.Mail;
using App.Shared.Util.ExceptionHandling.Types;
using App.WebApi.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace App.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(AppDbContext appDbContext, TokenHelper tokenHelper, IMailService emailService, IConfiguration configuration) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await appDbContext.Users
                    .Include(x => x.Role)
                    .Include(x => x.RefreshTokens)
                    .SingleOrDefaultAsync(x => x.Email == loginDto.Email) 
                    ?? throw new NotFoundException("User not found");

            var passwordValid = HashingHelper.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt);
            if (!passwordValid)
                throw new ValidationException("Invalid password");

            if (!user.IsActive)
            {
                await SendVerifyEmail(appDbContext, emailService, user);
                throw new BadRequestException("Please verify your email");
            }

            await tokenHelper.RevokedOldRefreshTokens(user); //Logout olunca RefreshToken bir daha kullanılmasın diye.

            var token = tokenHelper.CreateAccessToken(user);
            var refreshToken = await tokenHelper.CreateRefreshTokenAsync(user);

            return Ok(new LoggedResponse
            {
                AccessToken = token,
                RefreshToken = refreshToken.Token
            });
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var email = await appDbContext.Users.SingleOrDefaultAsync(x => x.Email == registerDto.Email);
            if (email is not null)
                throw new ConflictException("Email already exists");

            HashingHelper.CreatePasswordHash(registerDto.Password, out var passwordHash, out var passwordSalt);
            var user = new User
            {
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                UserName = registerDto.UserName,
                RoleId = 2
            };

            await appDbContext.Users.AddAsync(user);
            await appDbContext.SaveChangesAsync();
            await SendVerifyEmail(appDbContext, emailService, user);

            return Ok();
        }
        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromQuery] VerifyEmailDto verifyEmailDto)
        {
            var user = await appDbContext.Users.SingleOrDefaultAsync(x => x.Email == verifyEmailDto.Email) 
                ?? throw new NotFoundException("User not found");

            if (user.VerificationCode != verifyEmailDto.Code)
                throw new ValidationException("Invalid verification code");

            user.IsActive = true;
            await appDbContext.SaveChangesAsync();

            return Ok();
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest forgotPasswordRequest)
        {
            var user = await appDbContext.Users.SingleOrDefaultAsync(x => x.Email == forgotPasswordRequest.Email)
                ?? throw new NotFoundException("User not found");

            var guid = Guid.NewGuid().ToString()[..8];

            HashingHelper.CreatePasswordHash(guid, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;

            appDbContext.Users.Update(user);
            await appDbContext.SaveChangesAsync();

            var mailMessage = "<p>Tihs your new passford. Plesa don't share your password with anyone!</p>" +
                             $"<h1><b>{guid}</b></h1>";
            await emailService.SendEmailAsync(user.Email, "Reset Password", mailMessage);

            return Ok();
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            var user = await appDbContext.Users.SingleOrDefaultAsync(x => x.Email == resetPasswordRequest.Email)
                ?? throw new NotFoundException("User not found");

            if (resetPasswordRequest.Password != resetPasswordRequest.PasswordRepeat)
                throw new ValidationException("Passwords do not match");

            HashingHelper.CreatePasswordHash(resetPasswordRequest.Password, out var passwordHash, out var passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            appDbContext.Users.Update(user);
            await appDbContext.SaveChangesAsync();

            return Ok();
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            var refreshToken = await appDbContext.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshTokenRequest.Token) 
                ?? throw new NotFoundException("Refresh token not found");

            if (refreshToken.Revoked is not null)
                throw new ValidationException("Refresh token revoked");

            if (refreshToken.ExpiresAt < DateTime.Now)
                throw new ValidationException("Refresh token expired");

            var user = await appDbContext.Users.SingleOrDefaultAsync(x => x.Id == refreshToken.UserId) 
                ?? throw new NotFoundException("User not found");

            var newRefreshToken = await tokenHelper.RotateRefreshToken(user, refreshToken);
            var newAccessToken = tokenHelper.CreateAccessToken(user);

            return Ok(new RefreshedTokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token
            });
        }

        private async Task SendVerifyEmail(AppDbContext authDbContext, IMailService emailService, User user)
        {
            var verificationCode = Guid.NewGuid().ToString()[..6];
            user.VerificationCode = verificationCode;
            await authDbContext.SaveChangesAsync();

            string domain = configuration["App:ApiDomain"] ?? "https://localhost:7087";

            string httpEncodeEmail = HttpUtility.UrlEncode(user.Email);
            string verificationLink = $"{domain}/api/Auth/verify-email?Email={httpEncodeEmail}&Code={verificationCode}";

            var emailBody = $"""
                            <h1>Welcome{user.UserName}</h1>
                            <p>Your verification code is: {verificationCode}</p>
                            <p>Click the link below to verify your email</p>
                            <a href='{verificationLink}'>Verify Email</a>
                            """;
            await emailService.SendEmailAsync(user.Email, "Welcome to MessagingApp!", emailBody);
        }
    }
}
