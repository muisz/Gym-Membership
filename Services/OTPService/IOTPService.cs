using GymMembership.Enums;
using GymMembership.Models;

namespace GymMembership.Services
{
    public interface IOTPService
    {
        public Task<OTP> CreateOTP(OTPUsageEnum usage, string destination);
        public Task DeactivatePreviousOTP(OTPUsageEnum usage, string destination);
        public Task<OTP?> GetOTP(OTPUsageEnum usage, string code, string destination);
        public Task Verify(OTP otp);
        public Task Deactivate(OTP otp);
    }
}