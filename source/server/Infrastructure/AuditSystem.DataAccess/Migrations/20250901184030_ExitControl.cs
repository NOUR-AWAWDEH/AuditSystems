using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ExitControl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExitControl_Risks_RiskId",
                table: "ExitControl");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskPrograms_RisksControls_RiskControlId",
                table: "RiskPrograms");

            migrationBuilder.DropForeignKey(
                name: "FK_RisksControls_Rates_RatingId",
                table: "RisksControls");

            migrationBuilder.DropForeignKey(
                name: "FK_RisksControls_Risks_RiskId",
                table: "RisksControls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RisksControls",
                table: "RisksControls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExitControl",
                table: "ExitControl");

            migrationBuilder.RenameTable(
                name: "RisksControls",
                newName: "RiskControls");

            migrationBuilder.RenameTable(
                name: "ExitControl",
                newName: "ExitControls");

            migrationBuilder.RenameIndex(
                name: "IX_RisksControls_RiskId",
                table: "RiskControls",
                newName: "IX_RiskControls_RiskId");

            migrationBuilder.RenameIndex(
                name: "IX_RisksControls_RatingId",
                table: "RiskControls",
                newName: "IX_RiskControls_RatingId");

            migrationBuilder.RenameIndex(
                name: "IX_ExitControl_RiskId",
                table: "ExitControls",
                newName: "IX_ExitControls_RiskId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ExitControls",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RiskControls",
                table: "RiskControls",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExitControls",
                table: "ExitControls",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ExitControls_Inadequate",
                table: "ExitControls",
                column: "Inadequate");

            migrationBuilder.AddForeignKey(
                name: "FK_ExitControls_Risks_RiskId",
                table: "ExitControls",
                column: "RiskId",
                principalTable: "Risks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskControls_Rates_RatingId",
                table: "RiskControls",
                column: "RatingId",
                principalTable: "Rates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskControls_Risks_RiskId",
                table: "RiskControls",
                column: "RiskId",
                principalTable: "Risks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskPrograms_RiskControls_RiskControlId",
                table: "RiskPrograms",
                column: "RiskControlId",
                principalTable: "RiskControls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExitControls_Risks_RiskId",
                table: "ExitControls");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskControls_Rates_RatingId",
                table: "RiskControls");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskControls_Risks_RiskId",
                table: "RiskControls");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskPrograms_RiskControls_RiskControlId",
                table: "RiskPrograms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RiskControls",
                table: "RiskControls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExitControls",
                table: "ExitControls");

            migrationBuilder.DropIndex(
                name: "IX_ExitControls_Inadequate",
                table: "ExitControls");

            migrationBuilder.RenameTable(
                name: "RiskControls",
                newName: "RisksControls");

            migrationBuilder.RenameTable(
                name: "ExitControls",
                newName: "ExitControl");

            migrationBuilder.RenameIndex(
                name: "IX_RiskControls_RiskId",
                table: "RisksControls",
                newName: "IX_RisksControls_RiskId");

            migrationBuilder.RenameIndex(
                name: "IX_RiskControls_RatingId",
                table: "RisksControls",
                newName: "IX_RisksControls_RatingId");

            migrationBuilder.RenameIndex(
                name: "IX_ExitControls_RiskId",
                table: "ExitControl",
                newName: "IX_ExitControl_RiskId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ExitControl",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RisksControls",
                table: "RisksControls",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExitControl",
                table: "ExitControl",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExitControl_Risks_RiskId",
                table: "ExitControl",
                column: "RiskId",
                principalTable: "Risks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskPrograms_RisksControls_RiskControlId",
                table: "RiskPrograms",
                column: "RiskControlId",
                principalTable: "RisksControls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RisksControls_Rates_RatingId",
                table: "RisksControls",
                column: "RatingId",
                principalTable: "Rates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RisksControls_Risks_RiskId",
                table: "RisksControls",
                column: "RiskId",
                principalTable: "Risks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
