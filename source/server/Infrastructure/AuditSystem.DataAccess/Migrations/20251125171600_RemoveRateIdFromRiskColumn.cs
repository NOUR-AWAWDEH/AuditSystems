using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRateIdFromRiskColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Risks_Rates_RateId",
                table: "Risks");

            migrationBuilder.DropIndex(
                name: "IX_Risks_RateId",
                table: "Risks");

            migrationBuilder.DropColumn(
                name: "RateId",
                table: "Risks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RateId",
                table: "Risks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Risks_RateId",
                table: "Risks",
                column: "RateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Risks_Rates_RateId",
                table: "Risks",
                column: "RateId",
                principalTable: "Rates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
