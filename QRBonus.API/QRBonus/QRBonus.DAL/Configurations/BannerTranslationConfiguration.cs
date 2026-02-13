using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QRBonus.DAL.Models;

namespace QRBonus.DAL.Configurations;

public class BannerTranslationConfiguration : BaseConfiguration<BannerTranslation>
{
    public override void Configure(EntityTypeBuilder<BannerTranslation> builder)
    {
        base.Configure(builder);

        builder.ToTable("banner_translation");

        builder.Property(x => x.Name)
            .HasMaxLength(256)
            .IsRequired(); 
        
        builder.Property(x => x.Description)
            .HasMaxLength(4096)
            .IsRequired();

        builder.HasIndex(x => new { x.LanguageId, x.BannerId })
            .IsUnique();
    }
}
