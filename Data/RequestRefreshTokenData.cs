using System.ComponentModel.DataAnnotations;

namespace GymMembership.Data
{
    public class RequestRefreshTokenData
    {
        [Required]
        public string Token { get; set; } = string.Empty;
    }
}
