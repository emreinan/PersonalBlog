using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Shared.Services.Recaptcha;

public class RecaptchaValidator : IRecaptchaValidator
{
    private readonly string _secretKey;

    public RecaptchaValidator(IConfiguration configuration)
    {
        _secretKey = configuration["RecaptchaSettings:SecretKey"];
    }

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

            dynamic result = JsonConvert.DeserializeObject(responseString);
            return result.success == "true";
        }
    }
}
