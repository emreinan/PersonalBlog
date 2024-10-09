
using App.Data.Entities.Auth;

namespace App.Data.Entities;

public class Project : Entity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public Guid UserId { get; set; }

    public virtual User User { get; set; } = default!;
}