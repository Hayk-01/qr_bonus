using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QRBonus.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RegionIdinQrCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "region_id",
                table: "qr_code",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_qr_code_region_id",
                table: "qr_code",
                column: "region_id");

            migrationBuilder.AddForeignKey(
                name: "fk_qr_code_regions_region_id",
                table: "qr_code",
                column: "region_id",
                principalTable: "region",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql(@"DO $$
                                    DECLARE
                                        batch_size INT := 1000;
                                        updated_rows INT := 1;
                                    BEGIN
                                        WHILE updated_rows > 0 LOOP
                                            WITH batch AS (
                                                SELECT qr.ctid, c.region_id
                                                FROM qr_code qr
                                                JOIN product_campaign pc ON qr.product_campaign_id = pc.id
                                                JOIN campaign c ON pc.campaign_id = c.id
                                                WHERE qr.region_id IS NULL
                                                LIMIT batch_size
                                            )
                                            UPDATE qr_code
                                            SET region_id = batch.region_id
                                            FROM batch
                                            WHERE qr_code.ctid = batch.ctid;
                                            GET DIAGNOSTICS updated_rows = ROW_COUNT;
                                            RAISE NOTICE 'Updated % rows...', updated_rows;
                                        END LOOP;
                                    END $$;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_qr_code_regions_region_id",
                table: "qr_code");

            migrationBuilder.DropIndex(
                name: "ix_qr_code_region_id",
                table: "qr_code");

            migrationBuilder.DropColumn(
                name: "region_id",
                table: "qr_code");
        }
    }
}
