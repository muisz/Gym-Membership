namespace GymMembership.Data
{
    public class PackageData
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public float Price { get; set; }
        public int ActiveDays { get; set; }
    }
}