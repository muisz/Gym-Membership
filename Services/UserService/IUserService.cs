using GymMembership.Models;

namespace GymMembership.Services
{
    public interface IUserService
    {
        public Task<User> Register(string name, string email, string password);
        public Task<User> Authenticate(string email, string password);
        public Task<User?> GetUserFromEmail(string value);
    }
}