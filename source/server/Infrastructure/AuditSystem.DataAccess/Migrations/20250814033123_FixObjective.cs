using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixObjective : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Risks_Objectives_ObjectiveId",
                table: "Risks");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "RiskControlMatrices",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Objectives_Description",
                table: "Objectives",
                column: "Description");

            migrationBuilder.AddForeignKey(
                name: "FK_Risks_Objectives_ObjectiveId",
                table: "Risks",
                column: "ObjectiveId",
                principalTable: "Objectives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Risks_Objectives_ObjectiveId",
                table: "Risks");

            migrationBuilder.DropIndex(
                name: "IX_Objectives_Description",
                table: "Objectives");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "RiskControlMatrices",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddForeignKey(
                name: "FK_Risks_Objectives_ObjectiveId",
                table: "Risks",
                column: "ObjectiveId",
                principalTable: "Objectives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
