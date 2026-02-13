using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QRBonus.DAL.Models;

namespace QRBonus.DAL.Configurations;

public class BannerConfiguration : BaseConfiguration<Banner>
{
    public override void Configure(EntityTypeBuilder<Banner> builder)
    {
        base.Configure(builder);

        builder.ToTable("banner");

        builder.Property(x => x.Link)
            .HasMaxLength(1024);
    }
}
