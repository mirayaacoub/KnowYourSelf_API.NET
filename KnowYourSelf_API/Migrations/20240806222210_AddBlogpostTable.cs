using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KnowYourSelf_API.Migrations
{
    /// <inheritdoc />
    public partial class AddBlogpostTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blogposts",
                columns: table => new
                {
                    BlogpostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlogpostTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BlogpostContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlogpostImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TherapistId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogposts", x => x.BlogpostId);
                    table.ForeignKey(
                        name: "FK_Blogposts_Therapists_TherapistId",
                        column: x => x.TherapistId,
                        principalTable: "Therapists",
                        principalColumn: "TherapistId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blogposts_TherapistId",
                table: "Blogposts",
                column: "TherapistId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blogposts");
        }
    }
}
