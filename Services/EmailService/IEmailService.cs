using GymMembership.Models;

namespace GymMembership.Services
{
    public interface IEmailService
    {
        public Task SendOTPRegistrationEmail(OTP otp, User user);
    }
}