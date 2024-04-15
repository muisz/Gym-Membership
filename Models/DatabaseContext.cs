using Microsoft.EntityFrameworkCore;

namespace GymMembership.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
    }
}