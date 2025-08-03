using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixSubPRocess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskControlMatrices_SubProcesses_SubProcessId",
                table: "RiskControlMatrices");

            migrationBuilder.DropForeignKey(
                name: "FK_SubProcesses_AuditProcesses_AuditProcessId",
                table: "SubProcesses");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskControlMatrices_SubProcesses_SubProcessId",
                table: "RiskControlMatrices",
                column: "SubProcessId",
                principalTable: "SubProcesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubProcesses_AuditProcesses_AuditProcessId",
                table: "SubProcesses",
                column: "AuditProcessId",
                principalTable: "AuditProcesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskControlMatrices_SubProcesses_SubProcessId",
                table: "RiskControlMatrices");

            migrationBuilder.DropForeignKey(
                name: "FK_SubProcesses_AuditProcesses_AuditProcessId",
                table: "SubProcesses");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskControlMatrices_SubProcesses_SubProcessId",
                table: "RiskControlMatrices",
                column: "SubProcessId",
                principalTable: "SubProcesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubProcesses_AuditProcesses_AuditProcessId",
                table: "SubProcesses",
                column: "AuditProcessId",
                principalTable: "AuditProcesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
