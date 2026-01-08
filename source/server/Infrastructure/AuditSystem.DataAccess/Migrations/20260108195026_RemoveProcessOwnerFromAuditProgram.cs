using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProcessOwnerFromAuditProgram : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditPrograms_AppUsers_ProcessOwnerId",
                table: "AuditPrograms");

            migrationBuilder.DropIndex(
                name: "IX_AuditPrograms_ProcessOwnerId",
                table: "AuditPrograms");

            migrationBuilder.DropColumn(
                name: "ProcessOwnerId",
                table: "AuditPrograms");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProcessOwnerId",
                table: "AuditPrograms",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AuditPrograms_ProcessOwnerId",
                table: "AuditPrograms",
                column: "ProcessOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditPrograms_AppUsers_ProcessOwnerId",
                table: "AuditPrograms",
                column: "ProcessOwnerId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
