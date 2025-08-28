using System.ComponentModel.DataAnnotations;

namespace IdentityVerification.Api.Entities
{
    public class FormField
    {
        public int FieldID { get; set; }

        [Required, MaxLength(256)]
        public string FieldName { get; set; } = string.Empty;

        [Required, MaxLength(64)]
        public string FieldType { get; set; } = string.Empty;

        public bool IsRequired { get; set; }

        [MaxLength(512)]
        public string? ExpectedValue { get; set; }

        public int? SizeLimit { get; set; }

        public int CategoryID { get; set; }

        public bool IsActive { get; set; }

        // Navigation
        public FieldCategory? Category { get; set; }
        public ICollection<TemplateFormField> TemplateFormFields { get; set; } = new List<TemplateFormField>();
        public ICollection<UserResponse> UserResponses { get; set; } = new List<UserResponse>();
    }
}
