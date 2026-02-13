using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QRBonus.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UserSeeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "user",
                columns: new[] { "id", "created_date", "first_name", "is_deleted", "last_name", "modify_date", "password_hash", "user_name" },
                values: new object[] { 1L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Admin", false, "User", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ANijkOpvmBW5rlnLfPoybM5zNHgrYC8+xVMTEEDQAs/lOdgYkbr2W90wAe73v0kBQg==", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "user",
                keyColumn: "id",
                keyValue: 1L);
        }
    }
}
