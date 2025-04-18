using FluentValidation;

namespace App.Shared.Dto.ContactMessage;

public class ContactMessageAddDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Subject { get; set; }
    public required string Message { get; set; }
}
public class ContactMessageAddDtoValidator : AbstractValidator<ContactMessageAddDto>
{
    public ContactMessageAddDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Subject).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Message).NotEmpty();
    }
}
