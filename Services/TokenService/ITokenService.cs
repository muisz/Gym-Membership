using System.Security.Claims;
using GymMembership.Data;
using GymMembership.Models;

namespace GymMembership.Services
{
    public interface ITokenService
    {
        public TokenData CreatePair(User user);
        public string Create(User user);
        public string CreateRefresh(User user);
        public Task<User?> ClaimFromRefresh(string token);
        public int GetUserIdFromClaim(ClaimsPrincipal principal);
    }
}