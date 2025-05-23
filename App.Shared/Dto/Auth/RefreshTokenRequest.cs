﻿using FluentValidation;

namespace App.Shared.Dto.Auth;

public class RefreshTokenRequest
{
    public required string Token { get; set; }
}

public class RefreshTokenRequestValidator
        : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(x => x.Token).NotEmpty();
    }
}
