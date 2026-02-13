using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QRBonus.DAL.Models;

namespace QRBonus.DAL.Configurations;

public class CampaignTranslationConfiguration : BaseConfiguration<CampaignTranslation>
{
    public override void Configure(EntityTypeBuilder<CampaignTranslation> builder)
    {
        base.Configure(builder);

        builder.ToTable("campaign_translation");

        builder.Property(x => x.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.HasIndex(x => new { x.LanguageId, x.CampaignId })
            .IsUnique();
    }
}