using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QRBonus.DAL.Models;

namespace QRBonus.DAL.Configurations;

public class ProductCampaignConfiguration : BaseConfiguration<ProductCampaign>
{
    public override void Configure(EntityTypeBuilder<ProductCampaign> builder)
    {
        base.Configure(builder);
        builder.ToTable("product_campaign");

        builder.Property(x => x.ProductId)
            .IsRequired(); 
        
        builder.Property(x => x.CampaignId)
            .IsRequired();
    }
}
