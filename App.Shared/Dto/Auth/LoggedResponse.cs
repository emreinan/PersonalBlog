using FluentValidation;

namespace App.Shared.Dto.Auth;

public class LoggedResponse
{
    public string RefreshToken { get; set; }
}
