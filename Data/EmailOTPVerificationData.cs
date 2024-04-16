using System.ComponentModel.DataAnnotations;

namespace GymMembership.Data
{
    public class EmailOTPVerificationData
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string Code { get; set; } = string.Empty;
    }
}
