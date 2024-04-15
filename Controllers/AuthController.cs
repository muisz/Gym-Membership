using GymMembership.Data;
using GymMembership.Exceptions;
using GymMembership.Models;
using GymMembership.Services;
using Microsoft.AspNetCore.Mvc;

namespace GymMembership.Controllers
{
    [Route("/api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<CreatedData>> PostRegister(AuthRegisterData payload)
        {
            try
            {
                User user = await _userService.Register(payload.Name, payload.Email, payload.Password);
                return StatusCode(StatusCodes.Status201Created, new CreatedData{ Id = user.Id });
            }
            catch (HttpException error)
            {
                return Problem(error.Message, statusCode: error.StatusCode);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthData>> PostLogin(AuthLoginData payload)
        {
            try
            {
                User user = await _userService.Authenticate(payload.Email, payload.Password);
                if (!user.IsVerified)
                    throw new HttpException("email not verified. Please verify your email address");
                
                AuthData response = new AuthData
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Token = _tokenService.CreatePair(user),
                };
                return Ok(response);
            }
            catch (HttpException error)
            {
                return Problem(error.Message, statusCode: error.StatusCode);
            }
        }
    }
}