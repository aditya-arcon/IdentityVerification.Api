using System.ComponentModel.DataAnnotations;
using IdentityVerification.Api.Validation;

namespace IdentityVerification.Api.DTOs
{
    public class UserResponseDto
    {
        public int ResponseID { get; set; }
        public int SubmissionID { get; set; }
        public int FieldID { get; set; }
        public string? ValueText { get; set; }
        public decimal? ValueNumber { get; set; }
        public DateTime? ValueDate { get; set; }
        public string? ValueFile { get; set; }
    }

    [ExactlyOneValue(nameof(ValueText), nameof(ValueNumber), nameof(ValueDate), nameof(ValueFile))]
    public class CreateUserResponseDto
    {
        [Required] public int SubmissionID { get; set; }
        [Required] public int FieldID { get; set; }
        public string? ValueText { get; set; }
        public decimal? ValueNumber { get; set; }
        public DateTime? ValueDate { get; set; }
        [MaxLength(512)] public string? ValueFile { get; set; }
    }

    [ExactlyOneValue(nameof(ValueText), nameof(ValueNumber), nameof(ValueDate), nameof(ValueFile))]
    public class UpdateUserResponseDto
    {
        public string? ValueText { get; set; }
        public decimal? ValueNumber { get; set; }
        public DateTime? ValueDate { get; set; }
        [MaxLength(512)] public string? ValueFile { get; set; }
    }
}
