using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CreateEntitis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskPrograms_RiskControls_RiskControlId",
                table: "RiskPrograms");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecialProject_AuditUniverses_AuditUniverseId",
                table: "SpecialProject");

            migrationBuilder.DropForeignKey(
                name: "FK_SubProcesses_Processes_ProcessId",
                table: "SubProcesses");

            migrationBuilder.DropTable(
                name: "Processes");

            migrationBuilder.DropTable(
                name: "RiskControls");

            migrationBuilder.DropColumn(
                name: "Designation",
                table: "UserManagements");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "TaskManagements");

            migrationBuilder.DropColumn(
                name: "RiskFactor",
                table: "SpecificRiskFactors");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "RiskAssessments");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "PlanningReports");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "JobTimeAllocationReports");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "JobSchedulings");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "JobPrioritizations");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "InternalAuditConsolidationReports");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "ComplianceChecklists");

            migrationBuilder.DropColumn(
                name: "IdentifiedThrough",
                table: "ComplianceAuditLinks");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "AuditUniverseObjectives");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "AuditPlanSummaryReports");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "AuditJobs");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "AuditExceptionReports");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "AuditEngagements");

            migrationBuilder.RenameColumn(
                name: "Remarkcommants",
                table: "Remarks",
                newName: "RemarkCommants");

            migrationBuilder.RenameColumn(
                name: "SelectYear",
                table: "JobPrioritizations",
                newName: "SelectedYear");

            migrationBuilder.AddColumn<Guid>(
                name: "RiskFactorId",
                table: "SpecificRiskFactors",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "ProjectName",
                table: "SpecialProject",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "SpecialProject",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "IndustryUpdate",
                table: "AuditUniverses",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyUpdate",
                table: "AuditUniverses",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "BusinessObjective",
                table: "AuditUniverses",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "SpecialProjectId",
                table: "AuditUniverses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "AuditProcesses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuditSettingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProcessName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RatingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuditorSettingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditProcesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditProcesses_AuditorSettings_AuditorSettingsId",
                        column: x => x.AuditorSettingsId,
                        principalTable: "AuditorSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditProcesses_Ratings_RatingId",
                        column: x => x.RatingId,
                        principalTable: "Ratings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RisksControls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RiskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RatingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RisksControls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RisksControls_Ratings_RatingId",
                        column: x => x.RatingId,
                        principalTable: "Ratings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RisksControls_Risks_RiskId",
                        column: x => x.RiskId,
                        principalTable: "Risks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpecificRiskFactors_RiskFactorId",
                table: "SpecificRiskFactors",
                column: "RiskFactorId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditProcesses_AuditorSettingsId",
                table: "AuditProcesses",
                column: "AuditorSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditProcesses_RatingId",
                table: "AuditProcesses",
                column: "RatingId");

            migrationBuilder.CreateIndex(
                name: "IX_RisksControls_RatingId",
                table: "RisksControls",
                column: "RatingId");

            migrationBuilder.CreateIndex(
                name: "IX_RisksControls_RiskId",
                table: "RisksControls",
                column: "RiskId");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskPrograms_RisksControls_RiskControlId",
                table: "RiskPrograms",
                column: "RiskControlId",
                principalTable: "RisksControls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialProject_AuditUniverses_AuditUniverseId",
                table: "SpecialProject",
                column: "AuditUniverseId",
                principalTable: "AuditUniverses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecificRiskFactors_RiskFactors_RiskFactorId",
                table: "SpecificRiskFactors",
                column: "RiskFactorId",
                principalTable: "RiskFactors",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskPrograms_RisksControls_RiskControlId",
                table: "RiskPrograms");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecialProject_AuditUniverses_AuditUniverseId",
                table: "SpecialProject");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecificRiskFactors_RiskFactors_RiskFactorId",
                table: "SpecificRiskFactors");

            migrationBuilder.DropForeignKey(
                name: "FK_SubProcesses_AuditProcesses_ProcessId",
                table: "SubProcesses");

            migrationBuilder.DropTable(
                name: "AuditProcesses");

            migrationBuilder.DropTable(
                name: "RisksControls");

            migrationBuilder.DropIndex(
                name: "IX_SpecificRiskFactors_RiskFactorId",
                table: "SpecificRiskFactors");

            migrationBuilder.DropColumn(
                name: "RiskFactorId",
                table: "SpecificRiskFactors");

            migrationBuilder.DropColumn(
                name: "SpecialProjectId",
                table: "AuditUniverses");

            migrationBuilder.RenameColumn(
                name: "RemarkCommants",
                table: "Remarks",
                newName: "Remarkcommants");

            migrationBuilder.RenameColumn(
                name: "SelectedYear",
                table: "JobPrioritizations",
                newName: "SelectYear");

            migrationBuilder.AddColumn<string>(
                name: "Designation",
                table: "UserManagements",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SerialNumber",
                table: "TaskManagements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RiskFactor",
                table: "SpecificRiskFactors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ProjectName",
                table: "SpecialProject",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "SpecialProject",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<int>(
                name: "SerialNumber",
                table: "RiskAssessments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SerialNumber",
                table: "PlanningReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SerialNumber",
                table: "JobTimeAllocationReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SerialNumber",
                table: "JobSchedulings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SerialNumber",
                table: "JobPrioritizations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SerialNumber",
                table: "InternalAuditConsolidationReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SerialNumber",
                table: "ComplianceChecklists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "IdentifiedThrough",
                table: "ComplianceAuditLinks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "IndustryUpdate",
                table: "AuditUniverses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyUpdate",
                table: "AuditUniverses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "BusinessObjective",
                table: "AuditUniverses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<int>(
                name: "SerialNumber",
                table: "AuditUniverseObjectives",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SerialNumber",
                table: "AuditPlanSummaryReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SerialNumber",
                table: "AuditJobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SerialNumber",
                table: "AuditExceptionReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SerialNumber",
                table: "AuditEngagements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Processes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuditorSettingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RatingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuditSettingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProcessName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Processes_AuditorSettings_AuditorSettingsId",
                        column: x => x.AuditorSettingsId,
                        principalTable: "AuditorSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Processes_Ratings_RatingId",
                        column: x => x.RatingId,
                        principalTable: "Ratings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiskControls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RatingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RiskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskControls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskControls_Ratings_RatingId",
                        column: x => x.RatingId,
                        principalTable: "Ratings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RiskControls_Risks_RiskId",
                        column: x => x.RiskId,
                        principalTable: "Risks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Processes_AuditorSettingsId",
                table: "Processes",
                column: "AuditorSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_Processes_RatingId",
                table: "Processes",
                column: "RatingId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskControls_RatingId",
                table: "RiskControls",
                column: "RatingId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskControls_RiskId",
                table: "RiskControls",
                column: "RiskId");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskPrograms_RiskControls_RiskControlId",
                table: "RiskPrograms",
                column: "RiskControlId",
                principalTable: "RiskControls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialProject_AuditUniverses_AuditUniverseId",
                table: "SpecialProject",
                column: "AuditUniverseId",
                principalTable: "AuditUniverses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubProcesses_Processes_ProcessId",
                table: "SubProcesses",
                column: "ProcessId",
                principalTable: "Processes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
