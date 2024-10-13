using FluentValidation;

namespace App.Shared.Dto.Auth;

public class ResetPasswordRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string PasswordRepeat { get; set; } = null!;
}
public class ResetPasswordRequestValidator
        : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(4);
        RuleFor(x => x.PasswordRepeat).Equal(x => x.Password);
    }
}