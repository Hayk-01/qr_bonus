using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace QRBonus.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ProductCampaigntables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_errors",
                table: "errors");

            migrationBuilder.RenameTable(
                name: "errors",
                newName: "error");

            migrationBuilder.RenameIndex(
                name: "ix_errors_is_deleted",
                table: "error",
                newName: "ix_error_is_deleted");

            migrationBuilder.RenameIndex(
                name: "ix_errors_created_date",
                table: "error",
                newName: "ix_error_created_date");

            migrationBuilder.AddPrimaryKey(
                name: "pk_error",
                table: "error",
                column: "id");

            migrationBuilder.CreateTable(
                name: "campaign",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modify_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_campaign", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modify_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product_campaign",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_id = table.Column<long>(type: "bigint", nullable: false),
                    campaign_id = table.Column<long>(type: "bigint", nullable: false),
                    point = table.Column<long>(type: "bigint", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modify_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_campaign", x => x.id);
                    table.ForeignKey(
                        name: "fk_product_campaign_campaign_campaign_id",
                        column: x => x.campaign_id,
                        principalTable: "campaign",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_product_campaign_products_product_id",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_campaign_created_date",
                table: "campaign",
                column: "created_date");

            migrationBuilder.CreateIndex(
                name: "ix_campaign_is_deleted",
                table: "campaign",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "ix_product_created_date",
                table: "product",
                column: "created_date");

            migrationBuilder.CreateIndex(
                name: "ix_product_is_deleted",
                table: "product",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "ix_product_campaign_campaign_id",
                table: "product_campaign",
                column: "campaign_id");

            migrationBuilder.CreateIndex(
                name: "ix_product_campaign_created_date",
                table: "product_campaign",
                column: "created_date");

            migrationBuilder.CreateIndex(
                name: "ix_product_campaign_is_deleted",
                table: "product_campaign",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "ix_product_campaign_product_id",
                table: "product_campaign",
                column: "product_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_campaign");

            migrationBuilder.DropTable(
                name: "campaign");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropPrimaryKey(
                name: "pk_error",
                table: "error");

            migrationBuilder.RenameTable(
                name: "error",
                newName: "errors");

            migrationBuilder.RenameIndex(
                name: "ix_error_is_deleted",
                table: "errors",
                newName: "ix_errors_is_deleted");

            migrationBuilder.RenameIndex(
                name: "ix_error_created_date",
                table: "errors",
                newName: "ix_errors_created_date");

            migrationBuilder.AddPrimaryKey(
                name: "pk_errors",
                table: "errors",
                column: "id");
        }
    }
}
