using App.Shared.Dto.Auth;

namespace App.Client.Services.Auth;

public class HttpAuthService(IHttpClientFactory httpClientFactory) : IAuthService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("AuthApiClient");

    public async Task ForgotPasswordAsync(string email)
    {
        var emailRequest = new ForgotPasswordRequest { Email = email };
        var response = await _httpClient.PostAsJsonAsync("/api/Auth/ForgotPassword", new { emailRequest });
        response.EnsureSuccessStatusCode();
    }

    public async Task<TokenResponse> LoginAsync(LoginDto loginDto)
    {

        var response = await _httpClient.PostAsJsonAsync("/api/Auth/Login", new { loginDto.Email, loginDto.Password });
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
        return result;
    }

    public async Task<TokenResponse> RefreshTokenAsync(string refreshToken)
    {
        var refreshTokenRequest = new RefreshTokenRequest { Token = refreshToken };
        var response = await _httpClient.PostAsJsonAsync("/api/Auth/RefreshToken", new { refreshTokenRequest });
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
        return result;
    }

    public async Task<TokenResponse> RegisterAsync(RegisterDto registerDto)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/Auth/Register", new { registerDto.Email, registerDto.Password, registerDto.UserName });
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
        return result;
    }
}
