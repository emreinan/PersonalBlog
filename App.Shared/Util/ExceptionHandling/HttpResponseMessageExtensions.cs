using System.Net.Http.Json;
using System.Net;
using App.Shared.Util.ExceptionHandling.Types;

namespace App.Shared.Util.ExceptionHandling;

public static class HttpResponseMessageExtensions
{
    public static async Task EnsureSuccessStatusCodeWithProblemDetails(this HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
            return;

        if (response.Content.Headers.ContentLength > 0)
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            var message = problem?.Detail ?? "An error occurred.";

            throw response.StatusCode switch
            {
                HttpStatusCode.NotFound => new NotFoundException(message),
                HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden => new UnauthorizedAccessException(message),
                HttpStatusCode.Conflict => new ConflictException(message),
                HttpStatusCode.BadRequest => new BadRequestException(message),
                HttpStatusCode.UnprocessableEntity => new ValidationException(message),
                _ => new Exception(message)
            };
        }

        response.EnsureSuccessStatusCode();
    }
}
