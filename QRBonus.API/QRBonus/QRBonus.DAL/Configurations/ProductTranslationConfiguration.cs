using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QRBonus.DAL.Models;

namespace QRBonus.DAL.Configurations;

public class ProductTranslationConfiguration : BaseConfiguration<ProductTranslation>
{
    public override void Configure(EntityTypeBuilder<ProductTranslation> builder)
    {
        base.Configure(builder);

        builder.ToTable("product_translation");

        builder.Property(x => x.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.HasIndex(x => new { x.LanguageId, x.ProductId })
            .IsUnique(); 

    }
}