using FluentValidation;

namespace App.Shared.Dto.User;

public class UserUpdateDto
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
}
public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
{
    public UserUpdateDtoValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required.");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.").EmailAddress().WithMessage("Email is not valid.");
    }
}