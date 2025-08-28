namespace IdentityVerification.Api.Entities
{
    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; } = string.Empty;

        // Unique, already enforced in AppDbContext
        public string Email { get; set; } = string.Empty;

        public string? Phone { get; set; }

        // Roles: "Admin" or "User"
        public string Role { get; set; } = "User";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // NEW: password-based auth
        public string? PasswordHash { get; set; }   // Base64
        public string? PasswordSalt { get; set; }   // Base64
        public DateTime? PasswordUpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation
        public ICollection<Template> CreatedTemplates { get; set; } = new List<Template>();
        public ICollection<ResponseSubmission> ResponseSubmissions { get; set; } = new List<ResponseSubmission>();
    }
}
