using Microsoft.EntityFrameworkCore;

namespace GymMembership.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<OTP> OTPs { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<UserPackage> UserPackages { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
    }
}