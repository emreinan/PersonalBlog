using App.Data.Entities.Auth;

namespace App.Data.Entities.Data;

public class Project : Entity<int>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}