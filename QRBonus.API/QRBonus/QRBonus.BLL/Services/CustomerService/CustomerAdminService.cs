using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using QRBonus.BLL.Mappers;
using QRBonus.DAL;
using QRBonus.DTO.CustomerDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.BLL.Services.CustomerService
{
    public class CustomerAdminService : CustomerBaseService, ICustomerAdminService
    {
        public CustomerAdminService(AppDbContext db) : base(db)
        {
        }

        public async Task<Result> Delete(long id)
        {
            var customer = await _db.Customers.FirstOrDefaultAsync(x => x.Id == id);

            if (customer == null)
            {
                return Result.NotFound();
            }

            customer.IsDeleted = true;

            await _db.SaveChangesAsync();
            return Result.Success();
        }

      
    }
}
