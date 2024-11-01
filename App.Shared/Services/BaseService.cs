using App.Shared.Services.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace App.Shared.Services;

public class BaseService(IHttpClientFactory httpClientFactory)
{
    protected readonly HttpClient _dataHttpClient = httpClientFactory.CreateClient("DataApiClient");
    protected readonly HttpClient _authHttpClient = httpClientFactory.CreateClient("AuthApiClient");

    protected void DataClientGetToken(ITokenService tokenService)
    {
        var token = tokenService.GetAccessToken();
        _dataHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    protected void AuthClientGetToken(ITokenService tokenService)
    {
        var token = tokenService.GetAccessToken();
        _authHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}