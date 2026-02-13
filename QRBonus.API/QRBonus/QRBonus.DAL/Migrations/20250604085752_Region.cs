using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QRBonus.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Region : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "region_id",
                table: "product",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "region_id",
                table: "prize",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "region_id",
                table: "customer",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "region_id",
                table: "campaign",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "region_id",
                table: "banner",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "region",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    area_code = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modify_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_region", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "region_translation",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    region_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modify_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    language_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_region_translation", x => x.id);
                    table.ForeignKey(
                        name: "fk_region_translation_region_region_id",
                        column: x => x.region_id,
                        principalTable: "region",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "region",
                columns: new[] { "id", "area_code", "created_date", "is_deleted", "modify_date" },
                values: new object[] { 1L, "+995", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "region_translation",
                columns: new[] { "id", "created_date", "is_deleted", "language_id", "modify_date", "name", "region_id" },
                values: new object[,]
                {
                    { 1L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, 1L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Georgia", 1L },
                    { 2L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, 2L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Georgia", 1L }
                });

            migrationBuilder.CreateIndex(
                name: "ix_product_region_id",
                table: "product",
                column: "region_id");

            migrationBuilder.CreateIndex(
                name: "ix_prize_region_id",
                table: "prize",
                column: "region_id");

            migrationBuilder.CreateIndex(
                name: "ix_customer_region_id",
                table: "customer",
                column: "region_id");

            migrationBuilder.CreateIndex(
                name: "ix_campaign_region_id",
                table: "campaign",
                column: "region_id");

            migrationBuilder.CreateIndex(
                name: "ix_banner_region_id",
                table: "banner",
                column: "region_id");

            migrationBuilder.CreateIndex(
                name: "ix_region_area_code",
                table: "region",
                column: "area_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_region_created_date",
                table: "region",
                column: "created_date");

            migrationBuilder.CreateIndex(
                name: "ix_region_is_deleted",
                table: "region",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "ix_region_translation_created_date",
                table: "region_translation",
                column: "created_date");

            migrationBuilder.CreateIndex(
                name: "ix_region_translation_is_deleted",
                table: "region_translation",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "ix_region_translation_language_id_region_id",
                table: "region_translation",
                columns: new[] { "language_id", "region_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_region_translation_region_id",
                table: "region_translation",
                column: "region_id");

            migrationBuilder.AddForeignKey(
                name: "fk_banner_regions_region_id",
                table: "banner",
                column: "region_id",
                principalTable: "region",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_campaign_regions_region_id",
                table: "campaign",
                column: "region_id",
                principalTable: "region",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_customer_regions_region_id",
                table: "customer",
                column: "region_id",
                principalTable: "region",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_prize_regions_region_id",
                table: "prize",
                column: "region_id",
                principalTable: "region",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_product_regions_region_id",
                table: "product",
                column: "region_id",
                principalTable: "region",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql(@"
                UPDATE customer set region_id=1;
                UPDATE campaign set region_id=1;
                UPDATE prize set region_id=1;
                UPDATE product set region_id=1;
                UPDATE banner set region_id=1;
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_banner_regions_region_id",
                table: "banner");

            migrationBuilder.DropForeignKey(
                name: "fk_campaign_regions_region_id",
                table: "campaign");

            migrationBuilder.DropForeignKey(
                name: "fk_customer_regions_region_id",
                table: "customer");

            migrationBuilder.DropForeignKey(
                name: "fk_prize_regions_region_id",
                table: "prize");

            migrationBuilder.DropForeignKey(
                name: "fk_product_regions_region_id",
                table: "product");

            migrationBuilder.DropTable(
                name: "region_translation");

            migrationBuilder.DropTable(
                name: "region");

            migrationBuilder.DropIndex(
                name: "ix_product_region_id",
                table: "product");

            migrationBuilder.DropIndex(
                name: "ix_prize_region_id",
                table: "prize");

            migrationBuilder.DropIndex(
                name: "ix_customer_region_id",
                table: "customer");

            migrationBuilder.DropIndex(
                name: "ix_campaign_region_id",
                table: "campaign");

            migrationBuilder.DropIndex(
                name: "ix_banner_region_id",
                table: "banner");

            migrationBuilder.DropColumn(
                name: "region_id",
                table: "product");

            migrationBuilder.DropColumn(
                name: "region_id",
                table: "prize");

            migrationBuilder.DropColumn(
                name: "region_id",
                table: "customer");

            migrationBuilder.DropColumn(
                name: "region_id",
                table: "campaign");

            migrationBuilder.DropColumn(
                name: "region_id",
                table: "banner");
        }
    }
}
