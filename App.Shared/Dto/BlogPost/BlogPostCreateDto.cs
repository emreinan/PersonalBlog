﻿using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Shared.Dto.BlogPost;

public class BlogPostCreateDto

{
    public string Title { get; set; }
    public string Content { get; set; }
    public IFormFile Image { get; set; } 
}
public class BlogPostDtoValidator : AbstractValidator<BlogPostCreateDto>
{
    public BlogPostDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
        RuleFor(x => x.Content).NotEmpty().WithMessage("Content is required.");
        RuleFor(x => x.Image).NotNull().WithMessage("Image is required.");
    }
}

