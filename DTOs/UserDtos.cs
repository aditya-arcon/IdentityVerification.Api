using System.ComponentModel.DataAnnotations;

namespace IdentityVerification.Api.DTOs
{
    public class UserDto
    {
        public int UserID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Role { get; set; } = "User";
        public DateTime CreatedAt { get; set; }
    }

    public class CreateUserDto
    {
        [Required, MaxLength(128)] public string UserName { get; set; } = string.Empty;
        [Required, EmailAddress, MaxLength(256)] public string Email { get; set; } = string.Empty;
        [MaxLength(64)] public string? Phone { get; set; }
        [Required, MaxLength(64)] public string Role { get; set; } = "User";
    }

    public class UpdateUserDto
    {
        [Required, MaxLength(128)] public string UserName { get; set; } = string.Empty;
        [MaxLength(64)] public string? Phone { get; set; }
        [Required, MaxLength(64)] public string Role { get; set; } = "User";
    }
}
