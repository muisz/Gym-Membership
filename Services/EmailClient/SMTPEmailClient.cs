using System.Net;
using System.Net.Mail;

namespace GymMembership.Services
{
    public class SMTPEmailClient : IEmailClient
    {
        private readonly IConfiguration _configuration;
        private readonly SmtpClient smtpClient;

        public SMTPEmailClient(IConfiguration configuration)
        {
            _configuration = configuration;
            smtpClient = new SmtpClient
            {
                Host = configuration["SMTP:Host"] ?? "",
                Port = int.Parse(configuration["SMTP:Port"] ?? "0"),
                Credentials = new NetworkCredential(configuration["SMTP:Username"], configuration["SMTP:Password"]),
                EnableSsl = true,
            };
        }

        public Task Send(MailMessage message)
        {
            smtpClient.Send(message);
            return Task.CompletedTask;
        }

        public string GetSender()
        {
            return _configuration["SMTP:Sender"] ?? "";
        }
    }
}