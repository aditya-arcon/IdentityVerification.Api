namespace IdentityVerification.Api.Entities
{
    public class UserResponse
    {
        public int ResponseID { get; set; }

        public int SubmissionID { get; set; }
        public int FieldID { get; set; }

        public string? ValueText { get; set; }
        public decimal? ValueNumber { get; set; }
        public DateTime? ValueDate { get; set; }
        public string? ValueFile { get; set; }

        // Navigation
        public ResponseSubmission? Submission { get; set; }
        public FormField? Field { get; set; }
    }
}
