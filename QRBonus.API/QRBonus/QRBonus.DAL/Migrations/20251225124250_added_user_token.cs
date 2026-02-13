using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QRBonus.DAL.Migrations
{
    /// <inheritdoc />
    public partial class added_user_token : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "device_type",
                table: "customer",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "token",
                table: "customer",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "device_type",
                table: "customer");

            migrationBuilder.DropColumn(
                name: "token",
                table: "customer");
        }
    }
}
