using Microsoft.AspNetCore.Mvc;
using ShoeStore.API.Models.DTOs;
using ShoeStore.API.Services.Interfaces;

namespace ShoeStore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authServices;
        public AuthController(IAuthService authServices)
        {
            _authServices = authServices;
        }

        // ===================== REGISTER =====================
        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _authServices.RegisterAsync(dto);
            return Ok(result);
        }

        // ===================== VERIFY EMAIL =====================
        // GET: api/auth/verify-email?token=xxx
        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token)
        {
            var result = await _authServices.VerifyEmailAsync(token);
            return Ok(new { message = "Email verified successfully" });
        }

        // ===================== LOGIN =====================
        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authServices.LoginAsync(dto);
            return Ok(result);
        }

        // ===================== GOOGLE LOGIN =====================
        // POST: api/auth/google-login
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDto dto)
        {
            var result = await _authServices.GoogleLoginAsync(dto.IdToken);
            return Ok(result);
        }

        // ===================== REFRESH TOKEN =====================
        // POST: api/auth/refresh-token
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
        {
            var result = await _authServices.RefreshTokenAsync(dto.RefreshToken);
            return Ok(result);
        }
    }
}
