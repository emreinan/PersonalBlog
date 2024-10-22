using App.Client.Util.ExceptionHandling;
using App.Shared.Dto.Auth;

namespace App.Client.Services.Auth;

public class HttpAuthService(IHttpClientFactory httpClientFactory) : IAuthService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("AuthApiClient");

    public async Task ForgotPasswordAsync(string email)
    {
        var emailRequest = new ForgotPasswordRequest { Email = email };
        var response = await _httpClient.PostAsJsonAsync("/api/Auth/forgot-password", new { emailRequest });
        response.EnsureSuccessStatusCodeWithApiError();
    }

    public async Task<TokenResponse> LoginAsync(LoginDto loginDto)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/Auth/login", loginDto);
        response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
        return result;
    }

    public async Task<TokenResponse> RefreshTokenAsync(string refreshToken)
    {
        var refreshTokenRequest = new RefreshTokenRequest { Token = refreshToken };
        var response = await _httpClient.PostAsJsonAsync("/api/Auth/refresh-token", new { refreshTokenRequest });
        response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
        return result;
    }

    public async Task<TokenResponse> RegisterAsync(RegisterDto registerDto)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/Auth/register", registerDto);
        response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
        return result;
    }
}
