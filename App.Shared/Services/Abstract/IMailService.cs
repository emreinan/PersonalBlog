using Ardalis.Result;

namespace App.Shared.Services.Abstract
{
    public interface  IMailService
    {
        Task SendEmailAsync(string to, string subject, string htmlMessage);
    }
}
