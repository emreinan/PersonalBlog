﻿using App.Shared.Dto.File;
using Ardalis.Result;
using Microsoft.AspNetCore.Http;

namespace App.Shared.Services.File
{
    public interface IFileService
    {
        Task<Result<string>> UploadFileAsync(IFormFile file);
        Task<Result> DownloadFileAsync(string fileUrl);
        Task<Result> DeleteFileAsync(string fileUrl);
    }
}
