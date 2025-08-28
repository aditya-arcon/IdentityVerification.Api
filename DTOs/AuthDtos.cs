using System.ComponentModel.DataAnnotations;

namespace IdentityVerification.Api.DTOs
{
    public class TokenRequestDto
    {
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    }

    public class TokenResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAtUtc { get; set; }
    }
}
