using FluentValidation;

namespace App.Shared.Dto.Auth;

public class ResetPasswordRequest
{
    public required string Email { get; set; } 
    public required string Password { get; set; } 
    public required string PasswordRepeat { get; set; } 
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