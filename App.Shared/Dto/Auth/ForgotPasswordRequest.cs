using FluentValidation;

namespace App.Shared.Dto.Auth;

public class ForgotPasswordRequest
{
    public string Email { get; set; } = null!;
}
public class ForgotPasswordRequestValidator
    : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}