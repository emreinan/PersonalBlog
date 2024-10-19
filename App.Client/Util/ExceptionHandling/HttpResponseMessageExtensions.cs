
namespace App.Client.Util.ExceptionHandling;

public static class HttpResponseMessageExtensions
{
    public static async Task EnsureSuccessStatusCodeWithApiError(this HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var apiError = await response.Content.ReadFromJsonAsync<ApiError>();
            throw new ApiException(apiError);
        }
    }
}


