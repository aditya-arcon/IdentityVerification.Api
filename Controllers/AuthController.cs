using IdentityVerification.Api.DTOs;
using IdentityVerification.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace IdentityVerification.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>Issue JWT by email (legacy/compat).</summary>
        [HttpPost("token")]
        [AllowAnonymous]
        public async Task<ActionResult<TokenResponseDto>> Token([FromBody] TokenRequestDto dto, CancellationToken ct)
        {
            var token = await _authService.IssueTokenAsync(dto, ct);
            return Ok(new TokenResponseDto
            {
                Token = token,
                ExpiresAtUtc = DateTime.UtcNow.AddMinutes(120)
            });
        }

        /// <summary>Register a new user (email + password).</summary>
        [HttpPost("register")]
        [AllowAnonymous] // Change to [Authorize(Roles="Admin")] if you want restricted registration
        public async Task<ActionResult<object>> Register([FromBody] RegisterUserWithPasswordDto dto, CancellationToken ct)
        {
            var user = await _authService.RegisterAsync(dto, ct);
            return CreatedAtAction(nameof(Register), new { id = user.UserID }, new
            {
                user.UserID,
                user.UserName,
                user.Email,
                user.Role,
                user.CreatedAt,
                user.IsActive
            });
        }

        /// <summary>Login with email + password to receive a JWT.</summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<TokenResponseDto>> Login([FromBody] LoginWithPasswordDto dto, CancellationToken ct)
        {
            var token = await _authService.LoginWithPasswordAsync(dto, ct);
            return Ok(new TokenResponseDto
            {
                Token = token,
                ExpiresAtUtc = DateTime.UtcNow.AddMinutes(120)
            });
        }

        /// <summary>Change password for the current user.</summary>
        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto, CancellationToken ct)
        {
            var sub = User.FindFirst("sub")?.Value
                      ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(sub) || !int.TryParse(sub, out var userId))
                return Unauthorized();

            await _authService.ChangePasswordAsync(userId, dto, ct);
            return NoContent();
        }
    }

}
