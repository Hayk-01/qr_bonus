using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QRBonus.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DAL.Configurations
{
    public class CampaignPrizeConfiguration : BaseConfiguration<CampaignPrize>
    {
        public override void Configure(EntityTypeBuilder<CampaignPrize> builder)
        {
            base.Configure(builder);

            builder.ToTable("campaign_prize");

            builder.HasIndex(x => new { x.CampaignId, x.Position })
                .IsUnique();
        }
    }
}