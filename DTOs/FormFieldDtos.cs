using System.ComponentModel.DataAnnotations;

namespace IdentityVerification.Api.DTOs
{
    public class FormFieldDto
    {
        public int FieldID { get; set; }
        public string FieldName { get; set; } = string.Empty;
        public string FieldType { get; set; } = string.Empty;
        public bool IsRequired { get; set; }
        public string? ExpectedValue { get; set; }
        public int CategoryID { get; set; }
        public int? SizeLimit { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateFormFieldDto
    {
        [Required, MaxLength(256)] public string FieldName { get; set; } = string.Empty;
        [Required, MaxLength(64)] public string FieldType { get; set; } = string.Empty;
        public bool IsRequired { get; set; }
        [MaxLength(512)] public string? ExpectedValue { get; set; }
        public int? SizeLimit { get; set; }
        [Required] public int CategoryID { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateFormFieldDto : CreateFormFieldDto { }
}
