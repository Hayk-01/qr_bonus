using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace QRBonus.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CampaignPrizestable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "campaign_prize",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    campaign_id = table.Column<long>(type: "bigint", nullable: false),
                    prize_id = table.Column<long>(type: "bigint", nullable: false),
                    position = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modify_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_campaign_prize", x => x.id);
                    table.ForeignKey(
                        name: "fk_campaign_prize_campaign_campaign_id",
                        column: x => x.campaign_id,
                        principalTable: "campaign",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_campaign_prize_prizes_prize_id",
                        column: x => x.prize_id,
                        principalTable: "prize",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_campaign_prize_campaign_id_position",
                table: "campaign_prize",
                columns: new[] { "campaign_id", "position" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_campaign_prize_created_date",
                table: "campaign_prize",
                column: "created_date");

            migrationBuilder.CreateIndex(
                name: "ix_campaign_prize_is_deleted",
                table: "campaign_prize",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "ix_campaign_prize_prize_id",
                table: "campaign_prize",
                column: "prize_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "campaign_prize");
        }
    }
}
