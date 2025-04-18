using App.Shared.Util.ExceptionHandling.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace App.Shared.Util.ExceptionHandling;

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseExceptionHandler(options =>
        {
            options.Run(async context =>
            {
                var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

                var problemDetails = new ProblemDetails
                {
                    Status = exception switch
                    {
                        ValidationException => StatusCodes.Status422UnprocessableEntity,
                        NotFoundException => StatusCodes.Status404NotFound,
                        UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                        BadRequestException => StatusCodes.Status400BadRequest,
                        _ => StatusCodes.Status500InternalServerError
                    },
                    Title = exception?.GetType().Name,
                    Detail = exception?.Message,
                    Instance = context.Request.Path,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
                };

                context.Response.StatusCode = problemDetails.Status ?? 500;
                context.Response.ContentType = "application/problem+json";

                await context.Response.WriteAsJsonAsync(problemDetails);
            });
        });
    }
}