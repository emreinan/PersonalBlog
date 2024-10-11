using App.Shared.Dto.File;
using App.Shared.Services.Abstract;
using FluentValidation;
using System.Reflection;


namespace App.File.Api.Services;

public static class Extensions
{
    public static IServiceCollection AddFileService(this IServiceCollection services)
    {
        services.AddScoped<IFileService, FileService>();
        //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddScoped<IValidator<FileUploadRequest>, FileUploadRequestValidator>();
        services.AddScoped<IValidator<FileDownloadRequest>, FileDownloadRequestValidator>();

        return services;
    }
}
