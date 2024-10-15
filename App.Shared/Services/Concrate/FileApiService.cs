using App.Shared.Services.Abstract;
using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace App.Shared.Services.Concrate;

public class FileApiService(IHttpClientFactory httpClientFactory) : IFileService
{
    public async Task<Result> DeleteFileAsync(string fileName)
    {
        var client = httpClientFactory.CreateClient("FileApiClient");

        var response = await client.DeleteAsync($"/api/File/Delete?fileName={fileName}");

        if (!response.IsSuccessStatusCode)
            return Result.Error("Unexpected error occurred while deleting the file.");

        return Result.Success();
    }

    public async Task<Result> DownloadFileAsync(string fileName)
    {
        var client = httpClientFactory.CreateClient("FileApiClient");

        var response = await client.GetAsync($"/api/File/Download?fileName={fileName}");

        if (!response.IsSuccessStatusCode)
            return Result.Error("Unexpected error occurred while downloading the file.");

        return Result.Success();
    }


    public async Task<Result<string>> UploadFileAsync(IFormFile image)
    {
        var client = httpClientFactory.CreateClient("FileApiClient");
        var formData = new MultipartFormDataContent();

        using var fileStream = image.OpenReadStream();
        var fileContent = new StreamContent(fileStream);
        formData.Add(fileContent, "file", image.FileName);

        var response = await client.PostAsync("/api/File/Upload", formData);

        if (!response.IsSuccessStatusCode)
            return Result<string>.Error("Unexpected error occurred while uploading the file.");

        var fileUrl = await response.Content.ReadAsStringAsync();
        return Result<string>.Success(fileUrl);
    }
}
