using GymMembership.Exceptions;
using GymMembership.Models;
using Microsoft.EntityFrameworkCore;

namespace GymMembership.Services
{
    public class UserService : IUserService
    {
        private readonly DatabaseContext _context;
        private readonly IPasswordHasher _hasher;

        public UserService(DatabaseContext context, IPasswordHasher hasher)
        {
            _context = context;
            _hasher = hasher;
        }

        public async Task<User> Register(string name, string email, string password)
        {
            User? userWithSameEmail = await GetUserFromEmail(email);
            if (userWithSameEmail != null)
                throw new HttpException("email address already taken");
            
            User user = new User
            {
                Name = name,
                Email = email.ToLower(),
                Password = _hasher.Hash(password),
                IsVerified = false,
                CreatedAt = DateTime.Now.ToUniversalTime(),
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> Authenticate(string email, string password)
        {
            User? user = await GetUserFromEmail(email);
            if (user == null)
                throw new HttpException("email address not found", StatusCodes.Status404NotFound);
            
            if (!_hasher.Check(password, user.Password))
                throw new HttpException("wrong password");
            
            return user;
        }

        public async Task<User?> GetUserFromEmail(string value)
        {
            return await _context.Users.SingleOrDefaultAsync(user => user.Email == value.ToLower());
        }
    }
}