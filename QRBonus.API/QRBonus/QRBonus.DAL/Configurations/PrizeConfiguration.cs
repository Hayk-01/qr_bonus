
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QRBonus.DAL.Models;

namespace QRBonus.DAL.Configurations;
public class PrizeConfiguration : BaseConfiguration<Prize>
{
    public override void Configure(EntityTypeBuilder<Prize> builder)
    {
        base.Configure(builder);

        builder.ToTable("prize");

        builder.Property(x => x.Link)
            .HasMaxLength(1024);
    }
}