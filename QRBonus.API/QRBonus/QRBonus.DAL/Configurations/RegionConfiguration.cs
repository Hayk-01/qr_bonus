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
    public class RegionConfiguration : BaseConfiguration<Region>
    {
        public override void Configure(EntityTypeBuilder<Region> builder)
        {
            base.Configure(builder);

            builder.ToTable("region");

            builder.Property(x => x.AreaCode)
                .HasMaxLength(8)
                .IsRequired();

            builder.HasIndex(x => x.AreaCode)
                .IsUnique();
        }
    }
}