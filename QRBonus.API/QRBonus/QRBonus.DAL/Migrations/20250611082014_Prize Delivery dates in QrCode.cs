using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QRBonus.DAL.Migrations
{
    /// <inheritdoc />
    public partial class PrizeDeliverydatesinQrCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "prize_delivery_date",
                table: "qr_code",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "prize_receive_date",
                table: "qr_code",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "user_id",
                table: "qr_code",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_qr_code_user_id",
                table: "qr_code",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_qr_code_users_user_id",
                table: "qr_code",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_qr_code_users_user_id",
                table: "qr_code");

            migrationBuilder.DropIndex(
                name: "ix_qr_code_user_id",
                table: "qr_code");

            migrationBuilder.DropColumn(
                name: "prize_delivery_date",
                table: "qr_code");

            migrationBuilder.DropColumn(
                name: "prize_receive_date",
                table: "qr_code");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "qr_code");
        }
    }
}
