using System.ComponentModel.DataAnnotations;

namespace IdentityVerification.Api.Entities
{
    public class ResponseSubmission
    {
        public int SubmissionID { get; set; }

        public int UserID { get; set; }
        public int TemplateID { get; set; }

        public DateTime SubmittedAt { get; set; }

        [Required, MaxLength(64)]
        public string Status { get; set; } = "Pending";

        // Navigation
        public User? User { get; set; }
        public Template? Template { get; set; }
        public ICollection<UserResponse> UserResponses { get; set; } = new List<UserResponse>();
    }
}
