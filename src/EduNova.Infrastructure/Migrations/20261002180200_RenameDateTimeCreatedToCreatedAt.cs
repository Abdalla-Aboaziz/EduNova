using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduNova.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameDateTimeCreatedToCreatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateTimeCreated",
                table: "Lectures",
                newName: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Lectures",
                newName: "DateTimeCreated");
        }
    }
}
