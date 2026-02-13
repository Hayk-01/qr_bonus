
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
    public class QrCodeConfiguration : BaseConfiguration<QrCode>
    {
        public override void Configure(EntityTypeBuilder<QrCode> builder)
        {
            base.Configure(builder);

            builder.ToTable("qr_code");

            builder.Property(x => x.Value)
                .HasMaxLength(256)
                .IsRequired();  
        }
    }
}
