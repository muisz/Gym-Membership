using System.ComponentModel.DataAnnotations;

namespace GymMembership.Data
{
    public class RequestEmailOTPData
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
