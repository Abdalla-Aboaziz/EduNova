using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduNova.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LectureEnitites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UploadedFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    StoredFileName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FileExtension = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadedFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lectures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    DurationInSeconds = table.Column<int>(type: "int", nullable: false),
                    VideoFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ThumbnailFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UploadedById = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTimeCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lectures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lectures_UploadedFiles_ThumbnailFileId",
                        column: x => x.ThumbnailFileId,
                        principalTable: "UploadedFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lectures_UploadedFiles_VideoFileId",
                        column: x => x.VideoFileId,
                        principalTable: "UploadedFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DownloadedLectures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LectureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DownloadedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DownloadedLectures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DownloadedLectures_Lectures_LectureId",
                        column: x => x.LectureId,
                        principalTable: "Lectures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MyListItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LectureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyListItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyListItems_Lectures_LectureId",
                        column: x => x.LectureId,
                        principalTable: "Lectures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DownloadedLectures_LectureId",
                table: "DownloadedLectures",
                column: "LectureId");

            migrationBuilder.CreateIndex(
                name: "IX_DownloadedLectures_UserId_LectureId",
                table: "DownloadedLectures",
                columns: new[] { "UserId", "LectureId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_SubjectId_Order",
                table: "Lectures",
                columns: new[] { "SubjectId", "Order" });

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_ThumbnailFileId",
                table: "Lectures",
                column: "ThumbnailFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_VideoFileId",
                table: "Lectures",
                column: "VideoFileId");

            migrationBuilder.CreateIndex(
                name: "IX_MyListItems_LectureId",
                table: "MyListItems",
                column: "LectureId");

            migrationBuilder.CreateIndex(
                name: "IX_MyListItems_UserId_LectureId",
                table: "MyListItems",
                columns: new[] { "UserId", "LectureId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DownloadedLectures");

            migrationBuilder.DropTable(
                name: "MyListItems");

            migrationBuilder.DropTable(
                name: "Lectures");

            migrationBuilder.DropTable(
                name: "UploadedFiles");
        }
    }
}
