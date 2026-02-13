using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace QRBonus.DAL.Migrations
{
    /// <inheritdoc />
    public partial class BannerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "banner",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    link = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    expiration_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modify_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_banner", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "banner_translation",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    banner_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    description = table.Column<string>(type: "character varying(4048)", maxLength: 4048, nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modify_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    language_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_banner_translation", x => x.id);
                    table.ForeignKey(
                        name: "fk_banner_translation_banner_banner_id",
                        column: x => x.banner_id,
                        principalTable: "banner",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_banner_created_date",
                table: "banner",
                column: "created_date");

            migrationBuilder.CreateIndex(
                name: "ix_banner_is_deleted",
                table: "banner",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "ix_banner_translation_banner_id",
                table: "banner_translation",
                column: "banner_id");

            migrationBuilder.CreateIndex(
                name: "ix_banner_translation_created_date",
                table: "banner_translation",
                column: "created_date");

            migrationBuilder.CreateIndex(
                name: "ix_banner_translation_is_deleted",
                table: "banner_translation",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "ix_banner_translation_language_id_banner_id",
                table: "banner_translation",
                columns: new[] { "language_id", "banner_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "banner_translation");

            migrationBuilder.DropTable(
                name: "banner");
        }
    }
}
