
namespace App.Data.Entities;

public class Comment : Entity<int>
{
    public string Content { get; set; } = string.Empty;
    public bool IsApproved { get; set; } = false;
    public Guid UserId { get; set; }
    public Guid PostId { get; set; }

    public virtual BlogPost Post { get; set; } = default!;
}