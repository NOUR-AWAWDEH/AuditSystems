using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixAuditProcess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditProcesses_Ratings_RatingId",
                table: "AuditProcesses");

            migrationBuilder.DropForeignKey(
                name: "FK_SubProcesses_AuditProcesses_ProcessId",
                table: "SubProcesses");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AuditProcesses");

            migrationBuilder.RenameColumn(
                name: "ProcessId",
                table: "SubProcesses",
                newName: "AuditProcessId");

            migrationBuilder.RenameIndex(
                name: "IX_SubProcesses_ProcessId",
                table: "SubProcesses",
                newName: "IX_SubProcesses_AuditProcessId");

            migrationBuilder.RenameColumn(
                name: "RatingId",
                table: "AuditProcesses",
                newName: "SubDepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_AuditProcesses_RatingId",
                table: "AuditProcesses",
                newName: "IX_AuditProcesses_SubDepartmentId");

            migrationBuilder.AlterColumn<string>(
                name: "Particular",
                table: "SubProcesses",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ProcessName",
                table: "AuditProcesses",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditProcesses_SubDepartments_SubDepartmentId",
                table: "AuditProcesses",
                column: "SubDepartmentId",
                principalTable: "SubDepartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubProcesses_AuditProcesses_AuditProcessId",
                table: "SubProcesses",
                column: "AuditProcessId",
                principalTable: "AuditProcesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditProcesses_SubDepartments_SubDepartmentId",
                table: "AuditProcesses");

            migrationBuilder.DropForeignKey(
                name: "FK_SubProcesses_AuditProcesses_AuditProcessId",
                table: "SubProcesses");

            migrationBuilder.RenameColumn(
                name: "AuditProcessId",
                table: "SubProcesses",
                newName: "ProcessId");

            migrationBuilder.RenameIndex(
                name: "IX_SubProcesses_AuditProcessId",
                table: "SubProcesses",
                newName: "IX_SubProcesses_ProcessId");

            migrationBuilder.RenameColumn(
                name: "SubDepartmentId",
                table: "AuditProcesses",
                newName: "RatingId");

            migrationBuilder.RenameIndex(
                name: "IX_AuditProcesses_SubDepartmentId",
                table: "AuditProcesses",
                newName: "IX_AuditProcesses_RatingId");

            migrationBuilder.AlterColumn<string>(
                name: "Particular",
                table: "SubProcesses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "ProcessName",
                table: "AuditProcesses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AuditProcesses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditProcesses_Ratings_RatingId",
                table: "AuditProcesses",
                column: "RatingId",
                principalTable: "Ratings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubProcesses_AuditProcesses_ProcessId",
                table: "SubProcesses",
                column: "ProcessId",
                principalTable: "AuditProcesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
