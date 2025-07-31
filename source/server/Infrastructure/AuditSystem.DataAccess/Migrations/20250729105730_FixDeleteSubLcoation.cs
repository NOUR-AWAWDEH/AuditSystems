using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixDeleteSubLcoation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_SubLocations_SubLocationId",
                table: "Departments");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_SubLocations_SubLocationId",
                table: "Departments",
                column: "SubLocationId",
                principalTable: "SubLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_SubLocations_SubLocationId",
                table: "Departments");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_SubLocations_SubLocationId",
                table: "Departments",
                column: "SubLocationId",
                principalTable: "SubLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
