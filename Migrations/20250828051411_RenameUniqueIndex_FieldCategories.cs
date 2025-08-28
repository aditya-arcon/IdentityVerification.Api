using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityVerification.Api.Migrations
{
    /// <inheritdoc />
    public partial class RenameUniqueIndex_FieldCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "UX_FieldCategories_CategoryName",
                table: "FieldCategories",
                column: "CategoryName",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_FieldCategory_Name_Allowed",
                table: "FieldCategories",
                sql: "CategoryName IN ('Personal Information','Identity Document','Biometric Information')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_FieldCategories_CategoryName",
                table: "FieldCategories");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FieldCategory_Name_Allowed",
                table: "FieldCategories");
        }
    }
}
