using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QRBonus.DAL.Models;

namespace QRBonus.DAL.Configurations
{
    public class UserSessionConfiguration : BaseConfiguration<UserSession>
    {
        public override void Configure(EntityTypeBuilder<UserSession> builder)
        {
            base.Configure(builder);

            builder.ToTable("user_session");

        }
    }
}
