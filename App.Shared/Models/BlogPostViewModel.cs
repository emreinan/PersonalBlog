using System.Reflection.Metadata;

namespace App.Shared.Models;

public class BlogPostViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
    public List<BlogPostViewModel> RecentBlogs { get; set; } = new List<BlogPostViewModel>();
}

