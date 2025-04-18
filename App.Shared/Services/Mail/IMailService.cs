
namespace App.Shared.Services.Mail
{
    public interface IMailService
    {
        Task SendEmailAsync(string to, string subject, string htmlMessage);
    }
}
