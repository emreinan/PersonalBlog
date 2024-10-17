namespace App.Client.Models;

public class BlogPostViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string ImageUrl { get; set; }
    public List<CommentViewModel> Comments { get; set; }
}

