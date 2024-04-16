using GymMembership.Enums;
using GymMembership.Exceptions;
using GymMembership.Models;
using Microsoft.EntityFrameworkCore;

namespace GymMembership.Services
{
    public class OTPService : IOTPService
    {
        private readonly DatabaseContext _context;

        public OTPService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<OTP> CreateOTP(OTPUsageEnum usage, string destination)
        {
            await DeactivatePreviousOTP(usage, destination);

            Random random = new Random();
            string code = random.Next(1000, 9999).ToString();
            DateTime createdAt = DateTime.Now.ToUniversalTime();
            OTP otp = new OTP
            {
                Code = code,
                Destination = destination,
                Usage = usage,
                IsActive = true,
                ValidUntil = createdAt.AddMinutes(3).ToUniversalTime(),
                CreatedAt = createdAt,
            };
            await _context.OTPs.AddAsync(otp);
            await _context.SaveChangesAsync();
            return otp;
        }

        public async Task DeactivatePreviousOTP(OTPUsageEnum usage, string destination)
        {
            List<OTP> otps =  await _context.OTPs.Where(otp => 
                otp.Usage == usage && otp.Destination == destination && otp.IsActive
            ).ToListAsync();
            otps.ForEach(otp => otp.IsActive = false);
            await _context.SaveChangesAsync();
        }

        public async Task<OTP?> GetOTP(OTPUsageEnum usage, string code, string destination)
        {
            return await _context.OTPs.SingleOrDefaultAsync(otp =>
                otp.Code == code && otp.Destination == destination && otp.IsActive);
        }

        public Task Verify(OTP otp)
        {
            if (otp.ValidUntil < DateTime.Now)
                throw new HttpException("OTP expired");
            return Task.CompletedTask;
        }

        public async Task Deactivate(OTP otp)
        {
            otp.IsActive = false;
            await _context.SaveChangesAsync();
        }
    }
}