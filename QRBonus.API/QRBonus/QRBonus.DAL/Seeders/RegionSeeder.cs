using Microsoft.EntityFrameworkCore;
using QRBonus.DAL.Models;

namespace QRBonus.DAL.Seeders
{
    public class RegionSeeder
    {
        public static void SeedData(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Region>().HasData(new Region
            {
                Id = 1,
                CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                ModifyDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsDeleted = false,
                AreaCode = "+995"
            });

            modelBuilder.Entity<RegionTranslation>().HasData(new RegionTranslation
            {
                Id = 1,
                LanguageId = 1,
                RegionId = 1,
                Name = "Georgia",
                CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                ModifyDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsDeleted = false
            });
            modelBuilder.Entity<RegionTranslation>().HasData(new RegionTranslation
            {
                Id = 2,
                LanguageId = 2,
                RegionId = 1,
                Name = "Georgia",
                CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                ModifyDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsDeleted = false
            });
        }
    }
}
