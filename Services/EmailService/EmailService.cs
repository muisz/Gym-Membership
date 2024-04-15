using System.Net.Mail;
using GymMembership.Models;

namespace GymMembership.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailClient _emailClient;

        public EmailService(IEmailClient emailClient)
        {
            _emailClient = emailClient;
        }

        public async Task SendOTPRegistrationEmail(OTP otp, User user)
        {
            string expTime = otp.ValidUntil.ToString("dddd, dd MMMM yyyy hh:mm tt");
            string body = $"<body>Hi {user.Name}, your OTP code is {otp.Code}, valid until {expTime}</body>";
            string sender = _emailClient.GetSender();

            MailMessage message = new MailMessage(sender, user.Email)
            {
                Subject = "OTP Verification",
                Body = body,
                IsBodyHtml = true,
            };
            await _emailClient.Send(message);
        }
    }
}