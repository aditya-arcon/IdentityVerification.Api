using IdentityVerification.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityVerification.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Template> Templates => Set<Template>();
        public DbSet<FormField> FormFields => Set<FormField>();
        public DbSet<FieldCategory> FieldCategories => Set<FieldCategory>();
        public DbSet<TemplateFormField> TemplateFormFields => Set<TemplateFormField>();
        public DbSet<User> Users => Set<User>();
        public DbSet<ResponseSubmission> ResponseSubmissions => Set<ResponseSubmission>();
        public DbSet<UserResponse> UserResponses => Set<UserResponse>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User
            modelBuilder.Entity<User>(e =>
            {
                e.HasKey(u => u.UserID);
                e.Property(u => u.Email).IsRequired().HasMaxLength(256);
                e.HasIndex(u => u.Email).IsUnique();
                e.Property(u => u.UserName).IsRequired().HasMaxLength(128);
                e.Property(u => u.Role).IsRequired().HasMaxLength(64);
            });

            // Template with CreatedBy -> User.Email (alternate principal key)
            modelBuilder.Entity<Template>(e =>
            {
                e.HasKey(t => t.TemplateID);
                e.Property(t => t.TemplateName).IsRequired().HasMaxLength(256);
                e.Property(t => t.CreatedBy).IsRequired().HasMaxLength(256);
                e.Property(t => t.CreatedAt).HasColumnType("datetime(6)");
                e.Property(t => t.LastUpdated).HasColumnType("datetime(6)");

                e.HasOne(t => t.CreatedByUser)
                 .WithMany(u => u.CreatedTemplates)
                 .HasForeignKey(t => t.CreatedBy)
                 .HasPrincipalKey(u => u.Email);
            });

            // FieldCategory
            modelBuilder.Entity<FieldCategory>(e =>
            {
                e.HasKey(c => c.CategoryID);
                e.Property(c => c.CategoryName).IsRequired().HasMaxLength(128);

                // Use a custom UNIQUE index name to avoid clashing with any prior IX_*
                e.HasIndex(c => c.CategoryName)
                 .IsUnique()
                 .HasDatabaseName("UX_FieldCategories_CategoryName");

                e.Property(c => c.Description).HasMaxLength(1024);

                // DB-level whitelist (MySQL 8+ CHECK constraint)
                e.ToTable(tb => tb.HasCheckConstraint(
                    "CK_FieldCategory_Name_Allowed",
                    "CategoryName IN ('Personal Information','Identity Document','Biometric Information')"
                ));
            });

            // FormField
            modelBuilder.Entity<FormField>(e =>
            {
                e.HasKey(f => f.FieldID);
                e.Property(f => f.FieldName).IsRequired().HasMaxLength(256);
                e.Property(f => f.FieldType).IsRequired().HasMaxLength(64);
                e.Property(f => f.ExpectedValue).HasMaxLength(512);
                e.Property(f => f.SizeLimit);
                e.HasOne(f => f.Category)
                    .WithMany(c => c.FormFields)
                    .HasForeignKey(f => f.CategoryID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // TemplateFormField
            modelBuilder.Entity<TemplateFormField>(e =>
            {
                e.HasKey(tf => tf.TemplateFormFieldID);
                e.Property(tf => tf.HelpText).HasMaxLength(1024);
                e.HasOne(tf => tf.Template)
                    .WithMany(t => t.TemplateFormFields)
                    .HasForeignKey(tf => tf.TemplateID)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(tf => tf.FormField)
                    .WithMany(f => f.TemplateFormFields)
                    .HasForeignKey(tf => tf.FieldID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ResponseSubmission
            modelBuilder.Entity<ResponseSubmission>(e =>
            {
                e.HasKey(rs => rs.SubmissionID);
                e.Property(rs => rs.SubmittedAt).HasColumnType("datetime(6)");
                e.Property(rs => rs.Status).IsRequired().HasMaxLength(64);

                e.HasOne(rs => rs.User)
                    .WithMany(u => u.ResponseSubmissions)
                    .HasForeignKey(rs => rs.UserID)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(rs => rs.Template)
                    .WithMany(t => t.ResponseSubmissions)
                    .HasForeignKey(rs => rs.TemplateID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // UserResponse with check constraint enforcing exactly one value
            modelBuilder.Entity<UserResponse>(e =>
            {
                e.HasKey(ur => ur.ResponseID);
                e.Property(ur => ur.ValueText);
                e.Property(ur => ur.ValueNumber).HasColumnType("decimal(18,4)");
                e.Property(ur => ur.ValueDate).HasColumnType("datetime(6)");
                e.Property(ur => ur.ValueFile).HasMaxLength(512);

                e.HasOne(ur => ur.Submission)
                    .WithMany(rs => rs.UserResponses)
                    .HasForeignKey(ur => ur.SubmissionID)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(ur => ur.Field)
                    .WithMany(f => f.UserResponses)
                    .HasForeignKey(ur => ur.FieldID)
                    .OnDelete(DeleteBehavior.Restrict);

                // MySQL 8+ supports check constraints
                e.ToTable(tb => tb.HasCheckConstraint(
                    "CK_UserResponse_ExactlyOneValue",
                    "( (CASE WHEN ValueText IS NOT NULL THEN 1 ELSE 0 END) + " +
                    "  (CASE WHEN ValueNumber IS NOT NULL THEN 1 ELSE 0 END) + " +
                    "  (CASE WHEN ValueDate IS NOT NULL THEN 1 ELSE 0 END) + " +
                    "  (CASE WHEN ValueFile IS NOT NULL THEN 1 ELSE 0 END) ) = 1"));
            });
        }
    }
}
