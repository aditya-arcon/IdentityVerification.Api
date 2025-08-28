using System.ComponentModel.DataAnnotations;

namespace IdentityVerification.Api.DTOs
{
    public class RegisterUserWithPasswordDto
    {
        [Required, MaxLength(128)]
        public string UserName { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(32)]
        public string? Phone { get; set; }

        // Optional: allow Admin to create admins; otherwise default to "User"
        [MaxLength(64)]
        public string? Role { get; set; } = "User";

        // Password complexity: 8+ chars (adjust as needed)
        [Required, MinLength(8), MaxLength(128)]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginWithPasswordDto
    {
        [Required, EmailAddress, MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(8), MaxLength(128)]
        public string Password { get; set; } = string.Empty;
    }

    public class ChangePasswordDto
    {
        [Required, MinLength(8), MaxLength(128)]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required, MinLength(8), MaxLength(128)]
        public string NewPassword { get; set; } = string.Empty;
    }
}
    