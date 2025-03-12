using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixProgramsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Programsrograms_Ratings_RatingId",
                table: "Programsrograms");

            migrationBuilder.DropForeignKey(
                name: "FK_Programsrograms_RiskControls_RiskControlId",
                table: "Programsrograms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Programsrograms",
                table: "Programsrograms");

            migrationBuilder.RenameTable(
                name: "Programsrograms",
                newName: "Programs");

            migrationBuilder.RenameIndex(
                name: "IX_Programsrograms_RiskControlId",
                table: "Programs",
                newName: "IX_Programs_RiskControlId");

            migrationBuilder.RenameIndex(
                name: "IX_Programsrograms_RatingId",
                table: "Programs",
                newName: "IX_Programs_RatingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Programs",
                table: "Programs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Programs_Ratings_RatingId",
                table: "Programs",
                column: "RatingId",
                principalTable: "Ratings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Programs_RiskControls_RiskControlId",
                table: "Programs",
                column: "RiskControlId",
                principalTable: "RiskControls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Programs_Ratings_RatingId",
                table: "Programs");

            migrationBuilder.DropForeignKey(
                name: "FK_Programs_RiskControls_RiskControlId",
                table: "Programs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Programs",
                table: "Programs");

            migrationBuilder.RenameTable(
                name: "Programs",
                newName: "Programsrograms");

            migrationBuilder.RenameIndex(
                name: "IX_Programs_RiskControlId",
                table: "Programsrograms",
                newName: "IX_Programsrograms_RiskControlId");

            migrationBuilder.RenameIndex(
                name: "IX_Programs_RatingId",
                table: "Programsrograms",
                newName: "IX_Programsrograms_RatingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Programsrograms",
                table: "Programsrograms",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Programsrograms_Ratings_RatingId",
                table: "Programsrograms",
                column: "RatingId",
                principalTable: "Ratings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Programsrograms_RiskControls_RiskControlId",
                table: "Programsrograms",
                column: "RiskControlId",
                principalTable: "RiskControls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
