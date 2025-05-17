using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixUplode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "FileSize",
                table: "SupportingDocs",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<Guid>(
                name: "DriveFileId",
                table: "SupportingDocs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "SupportingDocs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SupportingDocs_UserId",
                table: "SupportingDocs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportingDocs_AspNetUsers_UserId",
                table: "SupportingDocs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportingDocs_AspNetUsers_UserId",
                table: "SupportingDocs");

            migrationBuilder.DropIndex(
                name: "IX_SupportingDocs_UserId",
                table: "SupportingDocs");

            migrationBuilder.DropColumn(
                name: "DriveFileId",
                table: "SupportingDocs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SupportingDocs");

            migrationBuilder.AlterColumn<int>(
                name: "FileSize",
                table: "SupportingDocs",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
