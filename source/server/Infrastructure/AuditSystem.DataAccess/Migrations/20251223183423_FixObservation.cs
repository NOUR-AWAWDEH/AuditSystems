using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixObservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionDate",
                table: "Observations");

            migrationBuilder.DropColumn(
                name: "ActionOwner",
                table: "Observations");

            migrationBuilder.DropColumn(
                name: "ActionPlan",
                table: "Observations");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Observations",
                newName: "Highlight");

            migrationBuilder.AlterColumn<string>(
                name: "Background",
                table: "Observations",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Observations",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "RateId",
                table: "Observations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Observations_RateId",
                table: "Observations",
                column: "RateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Observations_Rates_RateId",
                table: "Observations",
                column: "RateId",
                principalTable: "Rates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Observations_Rates_RateId",
                table: "Observations");

            migrationBuilder.DropIndex(
                name: "IX_Observations_RateId",
                table: "Observations");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Observations");

            migrationBuilder.DropColumn(
                name: "RateId",
                table: "Observations");

            migrationBuilder.RenameColumn(
                name: "Highlight",
                table: "Observations",
                newName: "Description");

            migrationBuilder.AlterColumn<string>(
                name: "Background",
                table: "Observations",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActionDate",
                table: "Observations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ActionOwner",
                table: "Observations",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ActionPlan",
                table: "Observations",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }
    }
}
