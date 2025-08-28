using System.ComponentModel.DataAnnotations;

namespace IdentityVerification.Api.Entities
{
    public class FieldCategory
    {
        public int CategoryID { get; set; }

        [Required, MaxLength(128)]
        public string CategoryName { get; set; } = string.Empty;

        [MaxLength(1024)]
        public string? Description { get; set; }

        // Navigation
        public ICollection<FormField> FormFields { get; set; } = new List<FormField>();
    }
}
