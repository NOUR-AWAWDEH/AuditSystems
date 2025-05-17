using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixUplods : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportingDocs_AspNetUsers_UserId",
                table: "SupportingDocs");

            migrationBuilder.AlterColumn<string>(
                name: "URL",
                table: "SupportingDocs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "SupportingDocs",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_SupportingDocs_DriveFileId",
                table: "SupportingDocs",
                column: "DriveFileId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportingDocs_AspNetUsers_UserId",
                table: "SupportingDocs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportingDocs_AspNetUsers_UserId",
                table: "SupportingDocs");

            migrationBuilder.DropIndex(
                name: "IX_SupportingDocs_DriveFileId",
                table: "SupportingDocs");

            migrationBuilder.AlterColumn<string>(
                name: "URL",
                table: "SupportingDocs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "SupportingDocs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportingDocs_AspNetUsers_UserId",
                table: "SupportingDocs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
