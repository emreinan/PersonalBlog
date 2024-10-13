﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Shared.Dto.Auth;

public class VerifyEmailDto
{
    public string Email { get; set; }
    public string Code { get; set; }
}

public class VerifyEmailDtoValidator
        : AbstractValidator<VerifyEmailDto>
{
    public VerifyEmailDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Code).NotEmpty();
    }
}