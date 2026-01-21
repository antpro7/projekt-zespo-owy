using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projekt.Data;
using projekt.Models;
using projekt.RequestModel;
using projekt.ResponseModel;
using projekt.Services.Interfaces;

namespace projekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;

        public AuthController(AppDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var user = await _authService.Authenticate(request.Email, request.Password);
            var auth = await _authService.RevokeTokens(user.Id);
            var tokens = await _authService.GenerateTokensForUser(user);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            return Ok(new UserResponseModel(user, tokens));
        }
        [HttpPost("logout/{userId}")]
        public async Task<IActionResult> Logout(int userId)
        {
            var result = await _authService.RevokeTokens(userId);
            if (!result)
            {
                return BadRequest(new { message = "Failed to logout" });
            }
            return Ok(new { message = "Logged out successfully" });
        }
        [AllowAnonymous]
        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var newTokens = await _authService.RefreshTokens(request.UserId, request.RefreshToken);
            if (newTokens == null)
            {
                return Unauthorized(new { message = "Invalid refresh token" });
            }
            return Ok(newTokens);
        }


    }
}