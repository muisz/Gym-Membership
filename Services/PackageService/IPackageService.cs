using GymMembership.Models;

namespace GymMembership.Services
{
    public interface IPackageService
    {
        public Task<ICollection<Package>> GetPackages();
        public Task<Package?> GetPackage(int id);
        public Task<ICollection<UserPackage>> GetPackagesFromUser(int id);
    }
}