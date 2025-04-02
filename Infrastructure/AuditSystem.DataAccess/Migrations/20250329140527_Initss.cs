using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AuditSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("2a56440e-cd4b-4592-93ca-3d8451c01d00"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("87f62659-8242-432c-87ed-b592b2810dea"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("ed1b256f-0cf8-41f8-88d2-6fe504f037f3"));

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("92c1a98e-8a3c-4c7e-b9cc-4f8321dcb31d"), null, "User", "USER" },
                    { new Guid("b94a0ed7-4f2a-46a5-a3a9-d2f4d6a4f5c3"), null, "Admin", "ADMIN" },
                    { new Guid("e6a0c0f7-8f9a-4661-b47d-c8a8a37c9d70"), null, "Auditor", "AUDITOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("92c1a98e-8a3c-4c7e-b9cc-4f8321dcb31d"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b94a0ed7-4f2a-46a5-a3a9-d2f4d6a4f5c3"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("e6a0c0f7-8f9a-4661-b47d-c8a8a37c9d70"));

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("2a56440e-cd4b-4592-93ca-3d8451c01d00"), null, "Admin", "ADMIN" },
                    { new Guid("87f62659-8242-432c-87ed-b592b2810dea"), null, "Auditor", "AUDITOR" },
                    { new Guid("ed1b256f-0cf8-41f8-88d2-6fe504f037f3"), null, "User", "USER" }
                });
        }
    }
}
