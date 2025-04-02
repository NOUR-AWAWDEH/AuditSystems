using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AuditSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Inits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0194ba8d-24f0-456b-b5b0-691a698255aa"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("03812349-e152-46e5-bb57-28378257178e"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("305ab32a-bd47-4e8c-bb5a-df1e52b2d9c8"));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { new Guid("0194ba8d-24f0-456b-b5b0-691a698255aa"), null, "Admin", "ADMIN" },
                    { new Guid("03812349-e152-46e5-bb57-28378257178e"), null, "User", "USER" },
                    { new Guid("305ab32a-bd47-4e8c-bb5a-df1e52b2d9c8"), null, "Auditor", "AUDITOR" }
                });
        }
    }
}
