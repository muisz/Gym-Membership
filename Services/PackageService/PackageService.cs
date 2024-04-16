using GymMembership.Models;
using Microsoft.EntityFrameworkCore;

namespace GymMembership.Services
{
    public class PackageService : IPackageService
    {
        private readonly DatabaseContext _context;

        public PackageService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Package>> GetPackages()
        {
            return await _context.Packages.Where(package => package.IsActive).ToListAsync();
        }

        public async Task<Package?> GetPackage(int id)
        {
            return await _context.Packages.SingleOrDefaultAsync(package => package.Id == id);
        }
    }
}