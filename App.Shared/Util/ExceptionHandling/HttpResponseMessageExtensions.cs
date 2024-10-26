﻿
using System.Net.Http.Json;

namespace App.Shared.Util.ExceptionHandling;

public static class HttpResponseMessageExtensions
{
    public static async Task EnsureSuccessStatusCodeWithApiError(this HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var apiError = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine($"API Error: {apiError}");
            response.EnsureSuccessStatusCode();
            Console.WriteLine( apiError );
        }
    }
}


