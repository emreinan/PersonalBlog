using App.Shared.Dto.Auth;
using Ardalis.Result;
using Microsoft.AspNetCore.Http;

namespace App.Auth.Api.Services;

public interface IAuthService
{
    Task<Result<LoggedResponse>> LoginAsync(LoginDto loginDto);
    Task<Result> RegisterAsync(RegisterDto registerDto);
    Task<Result> ForgotPasswordAsync(ForgotPasswordRequest forgotPasswordRequest);
    Task<Result> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest);
    Task<Result> VerifyEmailAsync(VerifyEmailDto verifyEmailDto);
    Task<Result<RefreshedTokenResponse>> RefrehsTokenAsync(RefreshTokenRequest refreshTokenRequest);
}
