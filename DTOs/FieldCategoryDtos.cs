using System.ComponentModel.DataAnnotations;
using IdentityVerification.Api.Validation;

namespace IdentityVerification.Api.DTOs
{
    public class FieldCategoryDto
    {
        public int CategoryID { get; set; }

        // Only one of the three allowed values
        public string CategoryName { get; set; } = string.Empty;

        [MaxLength(1024)]
        public string? Description { get; set; }
    }

    public class CreateFieldCategoryDto
    {
        [Required, MaxLength(128)]
        [AllowedCategoryName] // <— Enforce the whitelist
        public string CategoryName { get; set; } = string.Empty;

        [MaxLength(1024)]
        public string? Description { get; set; }
    }

    public class UpdateFieldCategoryDto
    {
        [Required, MaxLength(128)]
        [AllowedCategoryName]
        public string CategoryName { get; set; } = string.Empty;

        [MaxLength(1024)]
        public string? Description { get; set; }
    }
}
