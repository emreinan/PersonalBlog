namespace App.Shared.Dto.AboutMe;

public class AboutMeResponseDto
{
    public string Introduciton { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string? Cv { get; set; }
    public string? ImageUrl1 { get; set; }
    public string? ImageUrl2 { get; set; }
}
