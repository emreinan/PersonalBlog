using App.Shared.Dto.PersonalInfo;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Shared.Dto.PersonalInfo
{
    public class PersonalInfoDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
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
