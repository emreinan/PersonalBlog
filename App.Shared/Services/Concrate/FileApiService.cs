using App.Shared.Dto.File;
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
    private readonly HttpClient client = httpClientFactory.CreateClient("FileApiClient");
    public async Task<Result> DeleteFileAsync(string fileName)
    {
        var deleteRequestDto = new FileDeleteRequest { FileName = fileName };
        var response = await client.DeleteAsync($"/api/File/Delete?FileName={deleteRequestDto}");

        if (!response.IsSuccessStatusCode)
            return Result.Error("Unexpected error occurred while deleting the file.");

        return Result.Success();
    }

    public async Task<Result> DownloadFileAsync(string fileName)
    {
        var downloadRequestDto = new FileDownloadRequest { FileName = fileName };
        var response = await client.GetAsync($"/api/File/Download?FileName={downloadRequestDto}");

        if (!response.IsSuccessStatusCode)
            return Result.Error("Unexpected error occurred while downloading the file.");

        return Result.Success();
    }


    public async Task<Result<string>> UploadFileAsync(IFormFile file)
    {
        var formData = new MultipartFormDataContent();

        using var fileStream = file.OpenReadStream();
        var fileContent = new StreamContent(fileStream);
        formData.Add(fileContent, "file", file.FileName);

        var response = await client.PostAsync("/api/File/Upload", formData);

        if (!response.IsSuccessStatusCode)
            return Result.Error("Unexpected error occurred while uploading the file.");

        var fileName = await response.Content.ReadAsStringAsync();
        return Result.Success(fileName);

    }
}

