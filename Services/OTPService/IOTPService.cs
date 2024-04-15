using GymMembership.Enums;
using GymMembership.Models;

namespace GymMembership.Services
{
    public interface IOTPService
    {
        public Task<OTP> CreateOTP(OTPUsageEnum usage, string destination);
        public Task DeactivatePreviousOTP(OTPUsageEnum usage, string destination);
    }
}