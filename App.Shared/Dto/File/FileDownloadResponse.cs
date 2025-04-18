namespace App.Shared.Dto.File;

public class FileDownloadResponse
{
    public string FileName { get; set; } = default!;
    public string ContentType { get; set; } = default!;
    public byte[] FileContent { get; set; } = default!;
}
