using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace App.Shared.Dto.BlogPost;

public class BlogPostResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}


