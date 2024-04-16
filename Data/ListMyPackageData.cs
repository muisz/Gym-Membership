namespace GymMembership.Data
{
    public class ListMyPackageData
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime RenewAt { get; set; }
    }
}