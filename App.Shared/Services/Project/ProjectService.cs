using App.Shared.Dto.Project;
using App.Shared.Models;
using App.Shared.Services.Token;
using App.Shared.Util.ExceptionHandling;
using Ardalis.Result;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace App.Shared.Services.Project;

public class ProjectService(IHttpClientFactory httpClientFactory,ITokenService tokenService) : BaseService(httpClientFactory),IProjectService
{
    public async Task AddProjectAsync(ProjectAddDto projectAddDto)
    {
        using var content = new MultipartFormDataContent();
        content.Add(new StringContent(projectAddDto.Title), "Title");
        content.Add(new StringContent(projectAddDto.Description), "Description");

        if (projectAddDto.Image != null)
        {
            using var memoryStream = new MemoryStream();
            await projectAddDto.Image.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            var imageContent = new ByteArrayContent(memoryStream.ToArray());
            imageContent.Headers.ContentType = new MediaTypeHeaderValue(projectAddDto.Image.ContentType);
            content.Add(imageContent, "Image", projectAddDto.Image.FileName);
        }

        DataClientGetToken(tokenService);
        var response = await _dataHttpClient.PostAsync("/api/Project", content);
        await response.EnsureSuccessStatusCodeWithApiError();
    }

    public async Task DeleteProjectAsync(int id)
    {
        DataClientGetToken(tokenService);
        var response = await _dataHttpClient.DeleteAsync($"/api/Project/{id}");
        await response.EnsureSuccessStatusCodeWithApiError();
    }

    public async Task EditProjectAsync(int id, ProjectEditDto projectEditDto)
    {
        using var content = new MultipartFormDataContent();
        content.Add(new StringContent(projectEditDto.Title), "Title");
        content.Add(new StringContent(projectEditDto.Description), "Description");

        if (projectEditDto.Image != null)
        {
            using var memoryStream = new MemoryStream();
            await projectEditDto.Image.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            var imageContent = new ByteArrayContent(memoryStream.ToArray());
            imageContent.Headers.ContentType = new MediaTypeHeaderValue(projectEditDto.Image.ContentType);
            content.Add(imageContent, "Image", projectEditDto.Image.FileName);
        }

        DataClientGetToken(tokenService);
        var response = await _dataHttpClient.PutAsync($"/api/Project/{id}", content);
        await response.EnsureSuccessStatusCodeWithApiError();
    }

    public async Task<ProjectViewModel> GetProjectByIdAsync(int id)
    {
        var response = await _dataHttpClient.GetAsync($"/api/Project/{id}");
        await response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<ProjectViewModel>();
        return result;
    }

    public async Task<List<ProjectViewModel>> GetProjectsAsync()
    {
        var response = await _dataHttpClient.GetAsync("/api/Project");
        await response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<List<ProjectViewModel>>();
        return result;
    }

    public async Task MakeActiveProjectAsync(int id)
    {
        DataClientGetToken(tokenService);
        var response = await _dataHttpClient.PutAsync($"/api/Project/Active/{id}/", null);
        await response.EnsureSuccessStatusCodeWithApiError();
    }

    public async Task MakeInActiveProjectAsync(int id)
    {
        DataClientGetToken(tokenService);
        var response = await _dataHttpClient.PutAsync($"/api/Project/InActive/{id}/", null);
        await response.EnsureSuccessStatusCodeWithApiError();
    }
}