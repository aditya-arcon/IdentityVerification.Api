using IdentityVerification.Api.DTOs;
using IdentityVerification.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IdentityVerification.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth) => _auth = auth;

        /// <summary>
        /// Issues a JWT for the given email if a matching user exists.
        /// </summary>
        [HttpPost("token")]
        [ProducesResponseType(typeof(TokenResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Token([FromBody] TokenRequestDto request, CancellationToken ct)
        {
            var token = await _auth.IssueTokenAsync(request, ct);
            return Ok(token);
        }
    }
}
