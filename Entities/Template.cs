using System.ComponentModel.DataAnnotations;

namespace IdentityVerification.Api.Entities
{
    public class Template
    {
        public int TemplateID { get; set; }

        [Required, MaxLength(256)]
        public string TemplateName { get; set; } = string.Empty;

        /// <summary>
        /// Email of the creating user (FK to User.Email).
        /// </summary>
        [Required, MaxLength(256)]
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdated { get; set; }

        public bool AllowDocumentUpload { get; set; }
        public bool AllowCameraCapture { get; set; }
        public bool IsActive { get; set; }

        // Navigation
        public User? CreatedByUser { get; set; }
        public ICollection<TemplateFormField> TemplateFormFields { get; set; } = new List<TemplateFormField>();
        public ICollection<ResponseSubmission> ResponseSubmissions { get; set; } = new List<ResponseSubmission>();
    }
}
