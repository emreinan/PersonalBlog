using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace App.Shared.Dto.BlogPost;

public class BlogPostUpdateDto
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public IFormFile? Image { get; set; }
}

public class BlogPostUpdateDtoValidator : AbstractValidator<BlogPostUpdateDto>
{
    public BlogPostUpdateDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
        RuleFor(x => x.Content).NotEmpty().WithMessage("Content is required.");
    }
}

