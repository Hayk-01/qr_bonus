using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QRBonus.DAL.Models;

namespace QRBonus.DAL.Configurations;
public class ErrorConfiguration : BaseConfiguration<Error>
{
    public override void Configure(EntityTypeBuilder<Error> builder)
    {
        base.Configure(builder);

        builder.ToTable("error");

        builder.Property(x => x.Name)
            .HasMaxLength(256)
            .IsRequired();
    }
}
