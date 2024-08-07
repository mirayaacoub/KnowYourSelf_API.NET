using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KnowYourSelf_API.Migrations
{
    /// <inheritdoc />
    public partial class SeedUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "IsVerified", "Password", "Role", "UserImageUrl", "Username" },
                values: new object[] { 1, "example@example.com", true, "password", "Patient", "http://example.com/image.jpg", "ExampleUser" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);
        }
    }
}
