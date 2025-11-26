using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddActionOwnerAtAuditProgram : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ActionOwnerId",
                table: "AuditPrograms",
                type: "uuid",
                nullable: true);

            migrationBuilder.Sql(@"
                UPDATE ""AuditPrograms""
                SET ""ActionOwnerId"" = (SELECT ""Id"" FROM ""AspNetUsers"" LIMIT 1);
            ");
            
            // If no users exist, we can't enforce the constraint on existing data without deleting it or failing.
            // Assuming there is at least one user, or we delete audit programs that can't be assigned.
            migrationBuilder.Sql(@"DELETE FROM ""AuditPrograms"" WHERE ""ActionOwnerId"" IS NULL;");

            migrationBuilder.AlterColumn<Guid>(
                name: "ActionOwnerId",
                table: "AuditPrograms",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditPrograms_ActionOwnerId",
                table: "AuditPrograms",
                column: "ActionOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditPrograms_AspNetUsers_ActionOwnerId",
                table: "AuditPrograms",
                column: "ActionOwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditPrograms_AspNetUsers_ActionOwnerId",
                table: "AuditPrograms");

            migrationBuilder.DropIndex(
                name: "IX_AuditPrograms_ActionOwnerId",
                table: "AuditPrograms");

            migrationBuilder.DropColumn(
                name: "ActionOwnerId",
                table: "AuditPrograms");
        }
    }
}
