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
    public class CustomerConfiguration : BaseConfiguration<Customer>
    {
        public override void Configure(EntityTypeBuilder<Customer> builder)
        {
            base.Configure(builder);

            builder.ToTable("customer");

            builder.Property(x => x.FirstName)
                .HasMaxLength(64);

            builder.Property(x => x.LastName)
                .HasMaxLength(64);

            builder.Property(x => x.PhoneNumber)
                .HasMaxLength(16)
                .IsRequired();

            builder.Property(x => x.Email)
                .HasMaxLength(256);

            builder.Property(x => x.VerificationCode)
                .HasMaxLength(64);

            builder.HasIndex(x => x.PhoneNumber)
                .IsUnique();
        }
    }
}