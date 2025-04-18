using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Shared.Dto.Comment;

public class CommentDto
{
    public required string Content { get; set; }
    public Guid UserId { get; set; }
    public Guid PostId { get; set; }
}
public class CommentDtoValidator : AbstractValidator<CommentDto>
{
    public CommentDtoValidator()
    {
        RuleFor(x => x.Content).NotEmpty().MaximumLength(500);
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.PostId).NotEmpty();
    }
}
