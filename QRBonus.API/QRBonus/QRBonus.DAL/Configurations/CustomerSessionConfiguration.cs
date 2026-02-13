using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QRBonus.DAL.Models;

namespace QRBonus.DAL.Configurations
{
    public class CustomerSessionConfiguration : BaseConfiguration<CustomerSession>
    {
        public override void Configure(EntityTypeBuilder<CustomerSession> builder)
        {
            base.Configure(builder);

            builder.ToTable("customer_session");

        }
    }
}
