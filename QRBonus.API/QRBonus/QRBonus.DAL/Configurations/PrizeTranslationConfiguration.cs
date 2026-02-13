
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QRBonus.DAL.Models;

namespace QRBonus.DAL.Configurations;
public class PrizeTranslationConfiguration : BaseConfiguration<PrizeTranslation>
{
    public override void Configure(EntityTypeBuilder<PrizeTranslation> builder)
    {
        base.Configure(builder);

        builder.ToTable("prize_translation");

        builder.Property(x => x.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.HasIndex(x => new { x.LanguageId, x.PrizeId })
             .IsUnique();
    }
}