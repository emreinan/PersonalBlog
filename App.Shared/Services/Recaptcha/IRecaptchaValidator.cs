
namespace App.Shared.Services.Recaptcha
{
    public interface IRecaptchaValidator
    {
        Task<bool> ValidateRecaptchaAsync(string recaptchaResponse);
    }
}