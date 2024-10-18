using App.Shared.Dto.Auth;

namespace App.Client.Services.Auth;

public interface IAuthService
{
    public Task<TokenResponse> LoginAsync(LoginDto loginDto);
    public Task<TokenResponse> RegisterAsync(RegisterDto registerDto);
    public Task<TokenResponse> RefreshTokenAsync(string refreshToken);
    public Task ForgotPasswordAsync(string email);
}
