using Microsoft.EntityFrameworkCore;
using QRBonus.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DAL.Seeders
{
    public class UserSeeder
    {
        private const string _superUserPasswordHash =
            "ANijkOpvmBW5rlnLfPoybM5zNHgrYC8+xVMTEEDQAs/lOdgYkbr2W90wAe73v0kBQg==";

        public static void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                ModifyDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsDeleted = false,
                UserName = "admin",
                FirstName = "Admin",
                LastName = "User",
                PasswordHash = _superUserPasswordHash
            });
        }
    }
}
