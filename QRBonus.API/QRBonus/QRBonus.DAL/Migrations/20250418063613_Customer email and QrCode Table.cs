using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace QRBonus.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CustomeremailandQrCodeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "customer",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "prize",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    is_leaderboard = table.Column<bool>(type: "boolean", nullable: false),
                    link = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modify_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_prize", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "qr_code",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    value = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    product_campaign_id = table.Column<long>(type: "bigint", nullable: false),
                    prize_id = table.Column<long>(type: "bigint", nullable: true),
                    customer_id = table.Column<long>(type: "bigint", nullable: true),
                    win_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_win_received = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modify_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_qr_code", x => x.id);
                    table.ForeignKey(
                        name: "fk_qr_code_customer_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_qr_code_prize_prize_id",
                        column: x => x.prize_id,
                        principalTable: "prize",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_qr_code_product_campaign_product_campaign_id",
                        column: x => x.product_campaign_id,
                        principalTable: "product_campaign",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_prize_created_date",
                table: "prize",
                column: "created_date");

            migrationBuilder.CreateIndex(
                name: "ix_prize_is_deleted",
                table: "prize",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "ix_qr_code_created_date",
                table: "qr_code",
                column: "created_date");

            migrationBuilder.CreateIndex(
                name: "ix_qr_code_customer_id",
                table: "qr_code",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_qr_code_is_deleted",
                table: "qr_code",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "ix_qr_code_prize_id",
                table: "qr_code",
                column: "prize_id");

            migrationBuilder.CreateIndex(
                name: "ix_qr_code_product_campaign_id",
                table: "qr_code",
                column: "product_campaign_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "qr_code");

            migrationBuilder.DropTable(
                name: "prize");

            migrationBuilder.DropColumn(
                name: "email",
                table: "customer");
        }
    }
}
