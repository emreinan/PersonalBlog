namespace App.Data.Entities;

public class BlogPost : Entity<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public Guid AuthorId { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}