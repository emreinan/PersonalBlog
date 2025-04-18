using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace App.Shared.Dto.BlogPost;

public class BlogPostDto
{
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required IFormFile Image { get; set; } 
    public Guid AuthorId { get; set; }
}
public class BlogPostDtoValidator : AbstractValidator<BlogPostDto>
{
    public BlogPostDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
        RuleFor(x => x.Content).NotEmpty().WithMessage("Content is required.");
        RuleFor(x => x.Image).NotNull().WithMessage("Image is required.");
    }
}

