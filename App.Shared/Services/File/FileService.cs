using App.Shared.Dto.File;
using App.Shared.Util.ExceptionHandling;
using Microsoft.AspNetCore.Http;

namespace App.Shared.Services.File;

public class FileService(IHttpClientFactory httpClientFactory) : BaseService(httpClientFactory),IFileService
{
    public async Task DeleteFileAsync(string fileUrl)
    {
        var deleteRequestDto = new FileDeleteRequest { FileUrl = fileUrl };
        var response = await _apiHttpClient.DeleteAsync($"/api/File/Delete?FileUrl={deleteRequestDto}");
        await response.EnsureSuccessStatusCodeWithProblemDetails();
    }

    public async Task<Stream> GetDownloadFileAsync(string fileUrl)
    {
        var response = await _apiHttpClient.GetAsync($"/api/File/Download?FileUrl={Uri.EscapeDataString(fileUrl)}");
        await response.EnsureSuccessStatusCodeWithProblemDetails();
        return await response.Content.ReadAsStreamAsync();
    }

    public async Task<Stream> GetFileAsync(string fileUrl)
    {
        var response = await _apiHttpClient.GetAsync($"/api/File/GetImage?fileUrl={Uri.EscapeDataString(fileUrl)}");
        await response.EnsureSuccessStatusCodeWithProblemDetails();
        return await response.Content.ReadAsStreamAsync();
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        var formData = new MultipartFormDataContent();

        using var fileStream = file.OpenReadStream();
        var fileContent = new StreamContent(fileStream);
        formData.Add(fileContent, "File", file.FileName);

        var response = await _apiHttpClient.PostAsync("/api/File/Upload", formData);

        await response.EnsureSuccessStatusCodeWithProblemDetails();

        return await response.Content.ReadAsStringAsync();
    }
}

