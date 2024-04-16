namespace GymMembership.Models
{
    public class Package
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ActiveDays { get; set; }
        public bool IsActive { get; set; }
        public float Price { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<UserPackage> Users { get; } = new List<UserPackage>();
    }
}
