using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QRBonus.DAL.Models;

namespace QRBonus.DAL.Configurations
{
    public class RegionTranslationConfiguration : BaseConfiguration<RegionTranslation>
    {
        public override void Configure(EntityTypeBuilder<RegionTranslation> builder)
        {
            base.Configure(builder);

            builder.ToTable("region_translation");


            builder.Property(x => x.Name)
                .HasMaxLength(64)
                .IsRequired();

            builder.HasIndex(x => new { x.LanguageId, x.RegionId })
                .IsUnique();
        }
    }
}