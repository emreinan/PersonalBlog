using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;

namespace App.Shared.Services.Mail
{
    public class SmtpConfiguration
    {
        public required string Server { get; set; }
        public int Port { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public bool EnableSsl { get; set; }
    }
    public class SmtpEmailService : IMailService
    {
        private readonly SmtpConfiguration _smtpConfiguration;

        public SmtpEmailService(IOptions<SmtpConfiguration> smtpConfiguration)
        {
            _smtpConfiguration = smtpConfiguration.Value;
        }
        public async Task SendEmailAsync(string to, string subject, string htmlMessage)
        {
            try
            {
                var message = new MailMessage
                {
                    From = new MailAddress(_smtpConfiguration.Username),
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true
                };

                message.To.Add(to);

                var smtpClient = new SmtpClient(_smtpConfiguration.Server)
                {
                    Port = _smtpConfiguration.Port,
                    Credentials = new NetworkCredential(_smtpConfiguration.Username, _smtpConfiguration.Password),
                    EnableSsl = _smtpConfiguration.EnableSsl
                };

                await smtpClient.SendMailAsync(message);
            }
            catch (Exception e)
            {
                throw new Exception("Error sending email", e);
            }
        }
    }
}
