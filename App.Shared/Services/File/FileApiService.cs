﻿using App.Shared.Dto.File;
using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace App.Shared.Services.File;

public class FileApiService(IHttpClientFactory httpClientFactory) : IFileService
{
    private readonly HttpClient client = httpClientFactory.CreateClient("FileApiClient");
    public async Task<Result> DeleteFileAsync(string fileUrl)
    {
        var deleteRequestDto = new FileDeleteRequest { FileUrl = fileUrl };
        var response = await client.DeleteAsync($"/api/File/Delete?FileUrl={deleteRequestDto}");

        if (!response.IsSuccessStatusCode)
            return Result.Error("Unexpected error occurred while deleting the file.");

        return Result.Success();
    }

    public async Task<Stream> GetDownloadFileAsync(string fileUrl)
    {
        var response = await client.GetAsync($"/api/File/Download?FileUrl={Uri.EscapeDataString(fileUrl)}");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStreamAsync();
    }

    public async Task<Stream> GetFileAsync(string fileUrl)
    {
        var response = await client.GetAsync($"/api/File/GetImage?fileUrl={Uri.EscapeDataString(fileUrl)}");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStreamAsync();
    }

    public async Task<Result<string>> UploadFileAsync(IFormFile file)
    {
        var formData = new MultipartFormDataContent();

        using var fileStream = file.OpenReadStream();
        var fileContent = new StreamContent(fileStream);
        formData.Add(fileContent, "File", file.FileName);

        var response = await client.PostAsync("/api/File/Upload", formData);

        if (!response.IsSuccessStatusCode)
            return Result.Error("Unexpected error occurred while uploading the file.");

        var fileUrl = await response.Content.ReadAsStringAsync();
        return Result.Success(fileUrl);

    }
}

