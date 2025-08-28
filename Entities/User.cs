using System.ComponentModel.DataAnnotations;

namespace IdentityVerification.Api.Entities
{
    public class User
    {
        public int UserID { get; set; }

        [Required, MaxLength(128)]
        public string UserName { get; set; } = string.Empty;

        [Required, MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(64)]
        public string? Phone { get; set; }

        [Required, MaxLength(64)]
        public string Role { get; set; } = "User";

        public DateTime CreatedAt { get; set; }

        // Navigation
        public ICollection<Template> CreatedTemplates { get; set; } = new List<Template>();
        public ICollection<ResponseSubmission> ResponseSubmissions { get; set; } = new List<ResponseSubmission>();
    }
}
