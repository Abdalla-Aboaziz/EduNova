using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduNova.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBooksFeatureAndProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // SQL Server cannot implicitly convert int to uniqueidentifier,
            // and the old int values referenced no Subjects table (no FK existed),
            // so drop the column and re-add it with the new type.
            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Books");

            migrationBuilder.AddColumn<Guid>(
                name: "SubjectId",
                table: "Books",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_FileId",
                table: "Books",
                column: "FileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_SubjectId",
                table: "Books",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Subjects_SubjectId",
                table: "Books",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_UploadedFiles_FileId",
                table: "Books",
                column: "FileId",
                principalTable: "UploadedFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Subjects_SubjectId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_UploadedFiles_FileId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_FileId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_SubjectId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Books");

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "Books",
                type: "int",
                nullable: true);
        }
    }
}
