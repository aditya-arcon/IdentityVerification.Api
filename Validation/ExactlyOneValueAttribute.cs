using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace IdentityVerification.Api.Validation
{
    /// <summary>
    /// Validates that exactly one of the given property names on a model is non-null.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExactlyOneValueAttribute : ValidationAttribute
    {
        private readonly string[] _propertyNames;

        public ExactlyOneValueAttribute(params string[] propertyNames)
        {
            _propertyNames = propertyNames;
            ErrorMessage = $"Exactly one of: {string.Join(", ", propertyNames)} must be provided.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            var type = value.GetType();
            int count = 0;
            foreach (var name in _propertyNames)
            {
                var prop = type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
                if (prop == null) continue;
                var val = prop.GetValue(value);
                if (val != null) count++;
            }

            return count == 1 ? ValidationResult.Success : new ValidationResult(ErrorMessage);
        }
    }
}
