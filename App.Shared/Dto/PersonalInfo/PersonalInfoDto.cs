using App.Shared.Dto.PersonalInfo;
using FluentValidation;

namespace App.Shared.Dto.PersonalInfo
{
    public class PersonalInfoDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public required string Address { get; set; }
    }
}
public class PersonalInfoDtoValidator : AbstractValidator<PersonalInfoDto>
{
    public PersonalInfoDtoValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.");
        RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required.");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.");
        RuleFor(x => x.BirthDate).NotEmpty().WithMessage("Birth date is required.");
        RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required.");
    }
}
