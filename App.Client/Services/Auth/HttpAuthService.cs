using App.Client.Util.ExceptionHandling;
using App.Shared.Dto.Auth;

namespace App.Client.Services.Auth;

public class HttpAuthService(IHttpClientFactory httpClientFactory) : IAuthService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("AuthApiClient");

    public async Task ForgotPasswordAsync(string email)
    {
        var emailRequest = new ForgotPasswordRequest { Email = email };
        var response = await _httpClient.PostAsJsonAsync("/api/Auth/ForgotPassword", new { emailRequest });
        response.EnsureSuccessStatusCodeWithApiError();
    }

    public async Task<TokenResponse> LoginAsync(LoginDto loginDto)
    {

        var response = await _httpClient.PostAsJsonAsync("/api/Auth/Login", new { loginDto.Email, loginDto.Password });
        response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
        return result;
    }

    public async Task<TokenResponse> RefreshTokenAsync(string refreshToken)
    {
        var refreshTokenRequest = new RefreshTokenRequest { Token = refreshToken };
        var response = await _httpClient.PostAsJsonAsync("/api/Auth/RefreshToken", new { refreshTokenRequest });
        response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
        return result;
    }

    public async Task<TokenResponse> RegisterAsync(RegisterDto registerDto)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/Auth/Register", new { registerDto.Email, registerDto.Password, registerDto.UserName });
        response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
        return result;
    }
}
