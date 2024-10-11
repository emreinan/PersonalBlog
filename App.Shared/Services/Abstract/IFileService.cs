using App.Shared.Dto.File;
using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Shared.Services.Abstract
{
    public interface IFileService
    {
        Task<Result<FileUploadResponse>> UploadFileAsync(FileUploadRequest uploadFileRequest);
        Task<Result<FileDownloadResponse>> DownloadFileAsync(FileDownloadRequest fileDownloadRequest);
        Task<Result> DeleteFileAsync(string fileName);
    }
}
