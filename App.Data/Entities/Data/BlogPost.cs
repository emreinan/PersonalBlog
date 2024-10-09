
using App.Data.Entities.Auth;

namespace App.Data.Entities;

public class BlogPost : Entity
{
    public string Title { get; set; }
    public string Content { get; set; }
    public Guid UserId { get; set; }

    public virtual User User { get; set; } = default!;
    public virtual ICollection<Comment> Comments { get; set; } = default!;
}