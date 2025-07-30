using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ApplicationUser",
                keyColumn: "Id",
                keyValue: new Guid("25af36b5-0ccf-460c-a880-335d8e5eb6c8"));

            migrationBuilder.InsertData(
                table: "ApplicationUser",
                columns: new[] { "Id", "Email", "Fullname", "Password" },
                values: new object[] { new Guid("95049886-0868-42b3-98cf-d20dc8f5055d"), "admin@example.com", "Super Admin", "admin123" });

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 1,
                column: "UserId",
                value: new Guid("95049886-0868-42b3-98cf-d20dc8f5055d"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ApplicationUser",
                keyColumn: "Id",
                keyValue: new Guid("95049886-0868-42b3-98cf-d20dc8f5055d"));

            migrationBuilder.InsertData(
                table: "ApplicationUser",
                columns: new[] { "Id", "Email", "Fullname", "Password" },
                values: new object[] { new Guid("25af36b5-0ccf-460c-a880-335d8e5eb6c8"), "admin@example.com", "Super Admin", "admin123" });

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 1,
                column: "UserId",
                value: new Guid("25af36b5-0ccf-460c-a880-335d8e5eb6c8"));
        }
    }
}
