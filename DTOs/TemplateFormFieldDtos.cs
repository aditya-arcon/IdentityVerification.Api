using System.ComponentModel.DataAnnotations;

namespace IdentityVerification.Api.DTOs
{
    public class TemplateFormFieldDto
    {
        public int TemplateFormFieldID { get; set; }
        public int TemplateID { get; set; }
        public int FieldID { get; set; }
        public int FieldOrder { get; set; }
        public bool IsRequiredOverride { get; set; }
        public string? HelpText { get; set; }
    }

    public class CreateTemplateFormFieldDto
    {
        [Required] public int TemplateID { get; set; }
        [Required] public int FieldID { get; set; }
        public int FieldOrder { get; set; } = 0;
        public bool IsRequiredOverride { get; set; }
        [MaxLength(1024)] public string? HelpText { get; set; }
    }

    public class UpdateTemplateFormFieldDto
    {
        public int FieldOrder { get; set; }
        public bool IsRequiredOverride { get; set; }
        [MaxLength(1024)] public string? HelpText { get; set; }
    }
}
