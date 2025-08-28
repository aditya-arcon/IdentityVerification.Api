using System.ComponentModel.DataAnnotations;

namespace IdentityVerification.Api.DTOs
{
    public class TemplateDto
    {
        public int TemplateID { get; set; }
        public string TemplateName { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool AllowDocumentUpload { get; set; }
        public bool AllowCameraCapture { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateTemplateDto
    {
        [Required, MaxLength(256)] public string TemplateName { get; set; } = string.Empty;

        /// <summary>Creator's email (must exist in Users table).</summary>
        [Required, EmailAddress] public string CreatedBy { get; set; } = string.Empty;

        public bool AllowDocumentUpload { get; set; }
        public bool AllowCameraCapture { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateTemplateDto
    {
        [Required, MaxLength(256)] public string TemplateName { get; set; } = string.Empty;
        public bool AllowDocumentUpload { get; set; }
        public bool AllowCameraCapture { get; set; }
        public bool IsActive { get; set; }
    }
}
