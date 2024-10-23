namespace App.Data.Entities.Data;

public class AboutMe : Entity<int>
{
    public string Introduciton { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Cv { get; set; }
    public string? ImageUrl1 { get; set; }
    public string? ImageUrl2 { get; set; }
}
