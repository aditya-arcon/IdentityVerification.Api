using System.ComponentModel.DataAnnotations;

namespace IdentityVerification.Api.DTOs
{
    public class ResponseSubmissionDto
    {
        public int SubmissionID { get; set; }
        public int UserID { get; set; }
        public int TemplateID { get; set; }
        public DateTime SubmittedAt { get; set; }
        public string Status { get; set; } = "Pending";
    }

    public class CreateResponseSubmissionDto
    {
        [Required] public int UserID { get; set; }
        [Required] public int TemplateID { get; set; }
        [Required, MaxLength(64)] public string Status { get; set; } = "Pending";
    }

    public class UpdateResponseSubmissionDto
    {
        [Required, MaxLength(64)] public string Status { get; set; } = "Pending";
    }
}
