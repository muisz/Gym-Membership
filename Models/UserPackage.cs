using GymMembership.Enums;

namespace GymMembership.Models
{
    public class UserPackage
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int PackageId { get; set; }
        public Package Package { get; set; } = null!;
        public UserPackageStatusEnum Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime RenewAt { get; set; }
    }
}