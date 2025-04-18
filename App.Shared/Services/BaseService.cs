using App.Shared.Services.Token;
using System.Net.Http.Headers;

namespace App.Shared.Services;

public class BaseService(IHttpClientFactory httpClientFactory)
{
    protected readonly HttpClient _apiHttpClient = httpClientFactory.CreateClient("WebApiClient");

    protected void WebApiClientGetToken(ITokenService tokenService)
    {
        var token = tokenService.GetAccessToken();
        _apiHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}