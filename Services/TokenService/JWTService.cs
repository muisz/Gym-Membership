using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GymMembership.Data;
using GymMembership.Models;
using Microsoft.IdentityModel.Tokens;

namespace GymMembership.Services
{
    public class JWTService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly string accessKey;
        private readonly string refreshKey;
        private readonly string issuer;
        private readonly int accessExpHours;
        private readonly int refreshExpHours;

        public JWTService(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
            accessKey = configuration["JWT:AccessKey"] ?? "";
            refreshKey = configuration["JWT:RefreshKey"] ?? "";
            issuer = configuration["JWT:Issuer"] ?? "";
            accessExpHours = int.Parse(configuration["JWT:AccessExpHours"] ?? "3");
            refreshExpHours = int.Parse(configuration["JWT:RefreshExpHours"] ?? "3");
        }

        public TokenData CreatePair(User user)
        {
            string accessToken = Create(user);
            string refreshToken = CreateRefresh(user);
            return new TokenData{ Access = accessToken, Refresh = refreshToken };
        }

        public string Create(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
            };
            return CreateToken(claims, accessExpHours, accessKey);
        }

        public string CreateRefresh(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
            };
            return CreateToken(claims, refreshExpHours, refreshKey);
        }

        public async Task<User?> ClaimFromRefresh(string token)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(refreshKey));
            TokenValidationParameters parameters = new TokenValidationParameters
            {
                ValidIssuer = issuer,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal = handler.ValidateToken(token, parameters, out SecurityToken validatedToken);

            string? email = principal.Claims.SingleOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
            User? user = null;
            if (email != null)
                user = await _userService.GetUserFromEmail(email);
            return user;
        }

        private string CreateToken(ICollection<Claim> claims, int activeHours, string key)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(key));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            JwtSecurityToken token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(activeHours),
                signingCredentials: credentials,
                issuer: issuer
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}