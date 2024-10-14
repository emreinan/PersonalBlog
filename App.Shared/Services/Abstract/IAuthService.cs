using App.Shared.Dto.Auth;
using Ardalis.Result;

namespace App.Shared.Services.Abstract;

public interface IAuthService
{
    Task<Result<LoggedResponse>> LoginAsync(LoginDto loginDto);
    Task<Result> RegisterAsync(RegisterDto registerDto);
    Task<Result> ForgotPasswordAsync(ForgotPasswordRequest forgotPasswordRequest);
    Task<Result> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest);
    Task<Result> VerifyEmailAsync(VerifyEmailDto verifyEmailDto);
    Task<Result<RefreshedTokenResponse>> RefrehsTokenAsync(RefreshTokenRequest refreshTokenRequest);
}
