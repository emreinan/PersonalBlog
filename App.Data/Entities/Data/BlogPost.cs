using App.Data.Entities.Auth;

namespace App.Data.Entities.Data;

public class BlogPost : Entity<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;

    public virtual ICollection<Comment> Comments { get; set; } = default!;
}