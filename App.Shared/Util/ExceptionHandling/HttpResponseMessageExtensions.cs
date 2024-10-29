
using System.Net.Http.Json;
using System.Text.Json;

namespace App.Shared.Util.ExceptionHandling;

public static class HttpResponseMessageExtensions
{
    public static async Task EnsureSuccessStatusCodeWithApiError(this HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            if (response.Content.Headers.ContentLength > 0)
            {
                var apiError = await response.Content.ReadFromJsonAsync<ApiError>();
                throw new Exception(apiError?.Detail ?? "An error occurred while processing the request.");
            }
            else
            {
                throw new Exception("An error occurred while processing the request.");
            }
        }
    }
}


