using System.ComponentModel.DataAnnotations;

namespace IdentityVerification.Api.Entities
{
    public class TemplateFormField
    {
        public int TemplateFormFieldID { get; set; }

        public int TemplateID { get; set; }
        public int FieldID { get; set; }

        public int FieldOrder { get; set; }

        public bool IsRequiredOverride { get; set; }

        [MaxLength(1024)]
        public string? HelpText { get; set; }

        // Navigation
        public Template? Template { get; set; }
        public FormField? FormField { get; set; }
    }
}
