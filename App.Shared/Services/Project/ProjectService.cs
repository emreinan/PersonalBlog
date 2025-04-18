using App.Shared.Dto.Project;
using App.Shared.Models;
using App.Shared.Services.Token;
using App.Shared.Util.ExceptionHandling;
using App.Shared.Util.ExceptionHandling.Types;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace App.Shared.Services.Project;

public class ProjectService(IHttpClientFactory httpClientFactory,ITokenService tokenService) : BaseService(httpClientFactory),IProjectService
{
    public async Task AddProjectAsync(ProjectAddDto projectAddDto)
    {
        using var content = new MultipartFormDataContent
        {
            { new StringContent(projectAddDto.Title), "Title" },
            { new StringContent(projectAddDto.Description), "Description" }
        };

        if (projectAddDto.Image != null)
        {
            using var memoryStream = new MemoryStream();
            await projectAddDto.Image.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            var imageContent = new ByteArrayContent(memoryStream.ToArray());
            imageContent.Headers.ContentType = new MediaTypeHeaderValue(projectAddDto.Image.ContentType);
            content.Add(imageContent, "Image", projectAddDto.Image.FileName);
        }

        WebApiClientGetToken(tokenService);
        var response = await _apiHttpClient.PostAsync("/api/Project", content);
        await response.EnsureSuccessStatusCodeWithProblemDetails();
    }

    public async Task DeleteProjectAsync(int id)
    {
        WebApiClientGetToken(tokenService);
        var response = await _apiHttpClient.DeleteAsync($"/api/Project/{id}");
        await response.EnsureSuccessStatusCodeWithProblemDetails();
    }

    public async Task EditProjectAsync(int id, ProjectEditDto projectEditDto)
    {
        using var content = new MultipartFormDataContent
        {
            { new StringContent(projectEditDto.Title), "Title" },
            { new StringContent(projectEditDto.Description), "Description" }
        };

        if (projectEditDto.Image != null)
        {
            using var memoryStream = new MemoryStream();
            await projectEditDto.Image.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            var imageContent = new ByteArrayContent(memoryStream.ToArray());
            imageContent.Headers.ContentType = new MediaTypeHeaderValue(projectEditDto.Image.ContentType);
            content.Add(imageContent, "Image", projectEditDto.Image.FileName);
        }

        WebApiClientGetToken(tokenService);
        var response = await _apiHttpClient.PutAsync($"/api/Project/{id}", content);
        await response.EnsureSuccessStatusCodeWithProblemDetails();
    }

    public async Task<ProjectViewModel> GetProjectByIdAsync(int id)
    {
        var response = await _apiHttpClient.GetAsync($"/api/Project/{id}");
        await response.EnsureSuccessStatusCodeWithProblemDetails();
        var result = await response.Content.ReadFromJsonAsync<ProjectViewModel>() ??
            throw new DeserializationException("Failed to deserialize the response content.");
        return result;
    }

    public async Task<List<ProjectViewModel>> GetProjectsAsync()
    {
        var response = await _apiHttpClient.GetAsync("/api/Project");
        await response.EnsureSuccessStatusCodeWithProblemDetails();
        var result = await response.Content.ReadFromJsonAsync<List<ProjectViewModel>>() ??
            throw new DeserializationException("Failed to deserialize the response content.");
        return result;
    }

    public async Task MakeActiveProjectAsync(int id)
    {
        WebApiClientGetToken(tokenService);
        var response = await _apiHttpClient.PutAsync($"/api/Project/Active/{id}/", null);
        await response.EnsureSuccessStatusCodeWithProblemDetails();
    }

    public async Task MakeInActiveProjectAsync(int id)
    {
        WebApiClientGetToken(tokenService);
        var response = await _apiHttpClient.PutAsync($"/api/Project/InActive/{id}/", null);
        await response.EnsureSuccessStatusCodeWithProblemDetails();
    }
}