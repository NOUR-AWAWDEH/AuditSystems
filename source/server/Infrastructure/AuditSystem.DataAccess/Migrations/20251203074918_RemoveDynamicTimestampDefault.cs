using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDynamicTimestampDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("d2719f8e-8b4f-4c0e-a6b8-123456789abc"),
                column: "CreatedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "JobTitles",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "JobTitles",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "JobTitles",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "JobTitles",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "CreatedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "JobTitles",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "CreatedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "JobTitles",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "CreatedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rates",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rates",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rates",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rates",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "CreatedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rates",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "CreatedAt",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("d2719f8e-8b4f-4c0e-a6b8-123456789abc"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 3, 7, 47, 47, 642, DateTimeKind.Utc).AddTicks(4899));

            migrationBuilder.UpdateData(
                table: "JobTitles",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 3, 7, 47, 47, 651, DateTimeKind.Utc).AddTicks(1076));

            migrationBuilder.UpdateData(
                table: "JobTitles",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 3, 7, 47, 47, 651, DateTimeKind.Utc).AddTicks(1920));

            migrationBuilder.UpdateData(
                table: "JobTitles",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 3, 7, 47, 47, 651, DateTimeKind.Utc).AddTicks(1925));

            migrationBuilder.UpdateData(
                table: "JobTitles",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 3, 7, 47, 47, 651, DateTimeKind.Utc).AddTicks(1927));

            migrationBuilder.UpdateData(
                table: "JobTitles",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 3, 7, 47, 47, 651, DateTimeKind.Utc).AddTicks(1929));

            migrationBuilder.UpdateData(
                table: "JobTitles",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 3, 7, 47, 47, 651, DateTimeKind.Utc).AddTicks(1930));

            migrationBuilder.UpdateData(
                table: "Rates",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 3, 7, 47, 47, 659, DateTimeKind.Utc).AddTicks(2210));

            migrationBuilder.UpdateData(
                table: "Rates",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 3, 7, 47, 47, 659, DateTimeKind.Utc).AddTicks(2619));

            migrationBuilder.UpdateData(
                table: "Rates",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 3, 7, 47, 47, 659, DateTimeKind.Utc).AddTicks(2627));

            migrationBuilder.UpdateData(
                table: "Rates",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 3, 7, 47, 47, 659, DateTimeKind.Utc).AddTicks(2628));

            migrationBuilder.UpdateData(
                table: "Rates",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 3, 7, 47, 47, 659, DateTimeKind.Utc).AddTicks(2630));
        }
    }
}
