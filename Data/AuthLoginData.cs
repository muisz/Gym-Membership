using System.ComponentModel.DataAnnotations;

namespace GymMembership.Data
{
    public class AuthLoginData
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
    }
}
