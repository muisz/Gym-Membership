using GymMembership.Data;
using GymMembership.Enums;
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
        private readonly IOTPService _otpService;
        private readonly IEmailService _emailService;

        public AuthController(
            IUserService userService,
            ITokenService tokenService,
            IOTPService otpService,
            IEmailService emailService
        )
        {
            _userService = userService;
            _tokenService = tokenService;
            _otpService = otpService;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<CreatedData>> PostRegister(AuthRegisterData payload)
        {
            try
            {
                User user = await _userService.Register(payload.Name, payload.Email, payload.Password);
                OTP otp = await _otpService.CreateOTP(OTPUsageEnum.EmailVerification, user.Email);
                await _emailService.SendOTPRegistrationEmail(otp, user);
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

        [HttpPost("verification/send")]
        public async Task<ActionResult> PostSendVerification(RequestEmailOTPData payload)
        {
            try
            {
                User? user = await _userService.GetUserFromEmail(payload.Email);
                if (user == null)
                    throw new HttpException("email not found", StatusCodes.Status404NotFound);
                
                if (user.IsVerified)
                    throw new HttpException("email already verified");
                
                OTP otp = await _otpService.CreateOTP(OTPUsageEnum.EmailVerification, user.Email);
                await _emailService.SendOTPRegistrationEmail(otp, user);
                return Ok();
            }
            catch (HttpException error)
            {
                return Problem(error.Message, statusCode: error.StatusCode);
            }
        }

        [HttpPost("verification/verify")]
        public async Task<ActionResult<AuthData>> PostVerify(EmailOTPVerificationData payload)
        {
            try
            {
                OTP? otp = await _otpService.GetOTP(OTPUsageEnum.EmailVerification, payload.Code, payload.Email);
                if (otp == null)
                    throw new HttpException("OTP not found", StatusCodes.Status404NotFound);
                
                User? user = await _userService.GetUserFromEmail(payload.Email);
                if (user == null)
                    throw new HttpException("User not found", StatusCodes.Status404NotFound);
                
                await _userService.VerifyEmail(user);
                await _otpService.Deactivate(otp);
                
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

        [HttpPost("token/refresh")]
        public async Task<ActionResult<TokenData>> PostRefreshToken(RequestRefreshTokenData payload)
        {
            try
            {
                User? user = await _tokenService.ClaimFromRefresh(payload.Token);
                if (user == null)
                    throw new HttpException("User not found", StatusCodes.Status404NotFound);
                
                TokenData newToken = _tokenService.CreatePair(user);
                return Ok(newToken);
            }
            catch (HttpException error)
            {
                return Problem(error.Message, statusCode: error.StatusCode);
            }
            catch (Exception)
            {
                return Problem("Invalid token", statusCode: StatusCodes.Status400BadRequest);
            }
        }
    }
}