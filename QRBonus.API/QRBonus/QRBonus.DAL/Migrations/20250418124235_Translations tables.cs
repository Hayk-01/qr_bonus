using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace QRBonus.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Translationstables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "campaign");

            migrationBuilder.AlterColumn<DateTime>(
                name: "start_date",
                table: "campaign",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "end_date",
                table: "campaign",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateTable(
                name: "campaign_translation",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    campaign_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modify_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    language_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_campaign_translation", x => x.id);
                    table.ForeignKey(
                        name: "fk_campaign_translation_campaign_campaign_id",
                        column: x => x.campaign_id,
                        principalTable: "campaign",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "prize_translation",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    prize_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modify_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    language_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_prize_translation", x => x.id);
                    table.ForeignKey(
                        name: "fk_prize_translation_prize_prize_id",
                        column: x => x.prize_id,
                        principalTable: "prize",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "product_translation",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modify_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    language_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_translation", x => x.id);
                    table.ForeignKey(
                        name: "fk_product_translation_product_product_id",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_campaign_translation_campaign_id",
                table: "campaign_translation",
                column: "campaign_id");

            migrationBuilder.CreateIndex(
                name: "ix_campaign_translation_created_date",
                table: "campaign_translation",
                column: "created_date");

            migrationBuilder.CreateIndex(
                name: "ix_campaign_translation_is_deleted",
                table: "campaign_translation",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "ix_campaign_translation_language_id_campaign_id",
                table: "campaign_translation",
                columns: new[] { "language_id", "campaign_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_prize_translation_created_date",
                table: "prize_translation",
                column: "created_date");

            migrationBuilder.CreateIndex(
                name: "ix_prize_translation_is_deleted",
                table: "prize_translation",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "ix_prize_translation_language_id_prize_id",
                table: "prize_translation",
                columns: new[] { "language_id", "prize_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_prize_translation_prize_id",
                table: "prize_translation",
                column: "prize_id");

            migrationBuilder.CreateIndex(
                name: "ix_product_translation_created_date",
                table: "product_translation",
                column: "created_date");

            migrationBuilder.CreateIndex(
                name: "ix_product_translation_is_deleted",
                table: "product_translation",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "ix_product_translation_language_id_product_id",
                table: "product_translation",
                columns: new[] { "language_id", "product_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_product_translation_product_id",
                table: "product_translation",
                column: "product_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "campaign_translation");

            migrationBuilder.DropTable(
                name: "prize_translation");

            migrationBuilder.DropTable(
                name: "product_translation");

            migrationBuilder.AlterColumn<DateTime>(
                name: "start_date",
                table: "campaign",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "end_date",
                table: "campaign",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "campaign",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }
    }
}
