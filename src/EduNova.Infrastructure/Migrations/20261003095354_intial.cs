using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduNova.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class intial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lectures_Uploaded Files_ThumbnailFileId",
                table: "Lectures");

            migrationBuilder.DropForeignKey(
                name: "FK_Lectures_Uploaded Files_VideoFileId",
                table: "Lectures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Uploaded Files",
                table: "Uploaded Files");

            migrationBuilder.RenameTable(
                name: "Uploaded Files",
                newName: "UploadedFiles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UploadedFiles",
                table: "UploadedFiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lectures_UploadedFiles_ThumbnailFileId",
                table: "Lectures",
                column: "ThumbnailFileId",
                principalTable: "UploadedFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lectures_UploadedFiles_VideoFileId",
                table: "Lectures",
                column: "VideoFileId",
                principalTable: "UploadedFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lectures_UploadedFiles_ThumbnailFileId",
                table: "Lectures");

            migrationBuilder.DropForeignKey(
                name: "FK_Lectures_UploadedFiles_VideoFileId",
                table: "Lectures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UploadedFiles",
                table: "UploadedFiles");

            migrationBuilder.RenameTable(
                name: "UploadedFiles",
                newName: "Uploaded Files");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Uploaded Files",
                table: "Uploaded Files",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lectures_Uploaded Files_ThumbnailFileId",
                table: "Lectures",
                column: "ThumbnailFileId",
                principalTable: "Uploaded Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lectures_Uploaded Files_VideoFileId",
                table: "Lectures",
                column: "VideoFileId",
                principalTable: "Uploaded Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
