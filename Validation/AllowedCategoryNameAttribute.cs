using System.ComponentModel.DataAnnotations;

namespace IdentityVerification.Api.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class AllowedCategoryNameAttribute : ValidationAttribute
    {
        public static readonly string[] Allowed =
        {
            "Personal Information",
            "Identity Document",
            "Biometric Information"
        };

        public AllowedCategoryNameAttribute()
        {
            ErrorMessage = $"CategoryName must be one of: {string.Join(", ", Allowed)}.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext context)
        {
            if (value is null) return new ValidationResult(ErrorMessage);
            var s = value.ToString()?.Trim();
            if (string.IsNullOrEmpty(s)) return new ValidationResult(ErrorMessage);

            return Allowed.Contains(s) ? ValidationResult.Success : new ValidationResult(ErrorMessage);
        }
    }
}
