using App.Shared.Dto.Auth;
using App.Shared.Util.ExceptionHandling;
using App.Shared.Util.ExceptionHandling.Types;
using System.Net.Http.Json;

namespace App.Shared.Services.Auth;

public class HttpAuthService(IHttpClientFactory httpClientFactory) : BaseService(httpClientFactory),IAuthService 
{
    public async Task ForgotPasswordAsync(string email)
    {
        var emailRequest = new ForgotPasswordRequest { Email = email };
        var response = await _apiHttpClient.PostAsJsonAsync("/api/Auth/forgot-password", new { Email=emailRequest });
       await response.EnsureSuccessStatusCodeWithProblemDetails();
    }

    public async Task<TokenResponse> LoginAsync(LoginDto loginDto)
    {
        var response = await _apiHttpClient.PostAsJsonAsync("/api/Auth/login", loginDto);
      await  response.EnsureSuccessStatusCodeWithProblemDetails();
        var result = await response.Content.ReadFromJsonAsync<TokenResponse>() 
            ?? throw new DeserializationException("Failed to deserialize token response");
        return result;
    }

    public async Task<TokenResponse> RefreshTokenAsync(string refreshToken)
    {
        var refreshTokenRequest = new RefreshTokenRequest { Token = refreshToken };
        var response = await _apiHttpClient.PostAsJsonAsync("/api/Auth/refresh-token", new { refreshTokenRequest });
       await response.EnsureSuccessStatusCodeWithProblemDetails();
        var result = await response.Content.ReadFromJsonAsync<TokenResponse>() ??
            throw new DeserializationException("Failed to deserialize token response");
        return result;
    }

    public async Task<TokenResponse> RegisterAsync(RegisterDto registerDto)
    {
        var response = await _apiHttpClient.PostAsJsonAsync("/api/Auth/register", registerDto);
      await  response.EnsureSuccessStatusCodeWithProblemDetails();
        var result = await response.Content.ReadFromJsonAsync<TokenResponse>() ??
            throw new DeserializationException("Failed to deserialize token response");
        return result;
    }

    public async Task ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest)
    {
        var response = await _apiHttpClient.PostAsJsonAsync("/api/Auth/reset-password", resetPasswordRequest);
        await response.EnsureSuccessStatusCodeWithProblemDetails();
    }
}
