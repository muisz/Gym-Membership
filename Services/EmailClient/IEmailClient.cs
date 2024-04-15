using System.Net.Mail;

namespace GymMembership.Services
{
    public interface IEmailClient
    {
        public Task Send(MailMessage message);
        public string GetSender();
    }
}
