using App.Shared.Util.ExceptionHandling.Types;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace App.Shared.Services.Recaptcha;

public class RecaptchaValidator(IConfiguration configuration) : IRecaptchaValidator
{
    private readonly string _secretKey = configuration["RecaptchaSettings:SecretKey"]
                     ?? throw new ArgumentNullException(nameof(configuration), "RecaptchaSettings:SecretKey is not configured.");

    public async Task<bool> ValidateRecaptchaAsync(string recaptchaResponse)
    {
        using (var client = new HttpClient())
        {
            var values = new Dictionary<string, string>
                    {
                        { "secret", _secretKey },
                        { "response", recaptchaResponse }
                    };

            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
            var responseString = await response.Content.ReadAsStringAsync();

            dynamic result = JsonConvert.DeserializeObject(responseString) ??
                throw new DeserializationException("Failed to deserialize the response from Google reCAPTCHA.");
            return result.success == "true";
        }
    }
}
