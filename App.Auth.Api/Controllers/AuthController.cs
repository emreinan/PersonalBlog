using App.Shared.Dto.Auth;
using App.Shared.Services.Abstract;
using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<Result<LoggedResponse>> Login(LoginDto loginDto)
        {
            return await authService.LoginAsync(loginDto);
        }
        [HttpPost("register")]
        public async Task<Result> Register(RegisterDto registerDto)
        {
            return await authService.RegisterAsync(registerDto);
        }
        [HttpGet("verify-email")]
        public async Task<Result> VerifyEmail([FromQuery]VerifyEmailDto verifyEmailDto)
        {
            return await authService.VerifyEmailAsync(verifyEmailDto);
        }
        [HttpPost("forgot-password")]
        public async Task<Result> ForgotPassword(ForgotPasswordRequest forgotPasswordRequest)
        {
            return await authService.ForgotPasswordAsync(forgotPasswordRequest);
        }
        [HttpGet("reset-password")]
        public async Task<Result> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            return await authService.ResetPasswordAsync(resetPasswordRequest);
        }
        [HttpPost("refresh-token")]
        public async Task<Result<RefreshedTokenResponse>> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            return await authService.RefrehsTokenAsync(refreshTokenRequest);
        }
    }
}
