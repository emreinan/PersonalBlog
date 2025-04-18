using FluentValidation;

namespace App.Shared.Dto.Comment;

public class CommentUpdateDto
{
    public required string Content { get; set; }
}
public class CommentUpdateDtoValidator : AbstractValidator<CommentUpdateDto>
{
    public CommentUpdateDtoValidator()
    {
        RuleFor(x => x.Content).NotEmpty().WithMessage("Content is required.");
    }
}