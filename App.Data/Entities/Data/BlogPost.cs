using App.Data.Entities.Auth;

namespace App.Data.Entities.Data;

public class BlogPost : Entity<Guid>
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string ImageUrl { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = default!;
}