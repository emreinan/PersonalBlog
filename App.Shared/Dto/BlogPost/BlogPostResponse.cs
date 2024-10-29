using App.Shared.Dto.Comment;
using App.Shared.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace App.Shared.Dto.BlogPost;

public class BlogPostResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string ImageUrl { get; set; }
    public Guid AuthorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<CommentResponse> Comments { get; set; } = new List<CommentResponse>();
}


