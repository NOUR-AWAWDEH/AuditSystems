using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RefactorRiskToObservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Risks_Objectives_ObjectiveId",
                table: "Risks");

            migrationBuilder.DropIndex(
                name: "IX_Risks_ObjectiveId",
                table: "Risks");

            migrationBuilder.DropColumn(
                name: "ObjectiveId",
                table: "Risks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ObjectiveId",
                table: "Risks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Risks_ObjectiveId",
                table: "Risks",
                column: "ObjectiveId");

            migrationBuilder.AddForeignKey(
                name: "FK_Risks_Objectives_ObjectiveId",
                table: "Risks",
                column: "ObjectiveId",
                principalTable: "Objectives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
