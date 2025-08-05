using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixRiskControlMatrix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobPrioritizations_Ratings_RatingId",
                table: "JobPrioritizations");

            migrationBuilder.DropForeignKey(
                name: "FK_Objectives_Ratings_RatingId",
                table: "Objectives");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskPrograms_Ratings_RatingId",
                table: "RiskPrograms");

            migrationBuilder.DropForeignKey(
                name: "FK_Risks_Ratings_RatingId",
                table: "Risks");

            migrationBuilder.DropForeignKey(
                name: "FK_RisksControls_Ratings_RatingId",
                table: "RisksControls");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Objectives_RatingId",
                table: "Objectives");

            migrationBuilder.DropColumn(
                name: "RatingId",
                table: "Objectives");

            migrationBuilder.RenameColumn(
                name: "RatingId",
                table: "Risks",
                newName: "RateId");

            migrationBuilder.RenameIndex(
                name: "IX_Risks_RatingId",
                table: "Risks",
                newName: "IX_Risks_RateId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "RiskControlMatrices",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "ExitRisk",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RiskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotEnough = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExitRisk", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExitRisk_Risks_RiskId",
                        column: x => x.RiskId,
                        principalTable: "Risks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubProcesses_Particular",
                table: "SubProcesses",
                column: "Particular");

            migrationBuilder.CreateIndex(
                name: "IX_RiskControlMatrices_Description",
                table: "RiskControlMatrices",
                column: "Description");

            migrationBuilder.CreateIndex(
                name: "IX_ExitRisk_RiskId",
                table: "ExitRisk",
                column: "RiskId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobPrioritizations_Rates_RatingId",
                table: "JobPrioritizations",
                column: "RatingId",
                principalTable: "Rates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskPrograms_Rates_RatingId",
                table: "RiskPrograms",
                column: "RatingId",
                principalTable: "Rates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Risks_Rates_RateId",
                table: "Risks",
                column: "RateId",
                principalTable: "Rates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RisksControls_Rates_RatingId",
                table: "RisksControls",
                column: "RatingId",
                principalTable: "Rates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobPrioritizations_Rates_RatingId",
                table: "JobPrioritizations");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskPrograms_Rates_RatingId",
                table: "RiskPrograms");

            migrationBuilder.DropForeignKey(
                name: "FK_Risks_Rates_RateId",
                table: "Risks");

            migrationBuilder.DropForeignKey(
                name: "FK_RisksControls_Rates_RatingId",
                table: "RisksControls");

            migrationBuilder.DropTable(
                name: "ExitRisk");

            migrationBuilder.DropTable(
                name: "Rates");

            migrationBuilder.DropIndex(
                name: "IX_SubProcesses_Particular",
                table: "SubProcesses");

            migrationBuilder.DropIndex(
                name: "IX_RiskControlMatrices_Description",
                table: "RiskControlMatrices");

            migrationBuilder.RenameColumn(
                name: "RateId",
                table: "Risks",
                newName: "RatingId");

            migrationBuilder.RenameIndex(
                name: "IX_Risks_RateId",
                table: "Risks",
                newName: "IX_Risks_RatingId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "RiskControlMatrices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RatingId",
                table: "Objectives",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Objectives_RatingId",
                table: "Objectives",
                column: "RatingId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobPrioritizations_Ratings_RatingId",
                table: "JobPrioritizations",
                column: "RatingId",
                principalTable: "Ratings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Objectives_Ratings_RatingId",
                table: "Objectives",
                column: "RatingId",
                principalTable: "Ratings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskPrograms_Ratings_RatingId",
                table: "RiskPrograms",
                column: "RatingId",
                principalTable: "Ratings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Risks_Ratings_RatingId",
                table: "Risks",
                column: "RatingId",
                principalTable: "Ratings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RisksControls_Ratings_RatingId",
                table: "RisksControls",
                column: "RatingId",
                principalTable: "Ratings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
