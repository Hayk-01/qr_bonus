using InnLine.BLL.Filters;
using QRBonus.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.BLL.Filters
{
    public class CustomerFilter : BaseFilter<Customer>
    {
        public long? Id { get; set; }

        public List<long>? Ids { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public List<long>? RegionIds { get; set; }
        public override IQueryable<Customer> CreateQuery(IQueryable<Customer> query)
        {
            query = query.OrderByDescending(x => x.Id);
         
            if (Id.HasValue)
            {
                query = query.Where(x => x.Id == Id.Value);
            }

            if (Ids != null && Ids.Any())
            {
                query = query.Where(x => Ids.Contains(x.Id));
            }

            if (!string.IsNullOrWhiteSpace(FirstName))
            {
                query = query.Where(x => x.FirstName.ToLower().Replace(" ", "").Contains(FirstName.ToLower().Replace(" ", "")));
            }

            if (!string.IsNullOrWhiteSpace(LastName))
            {
                query = query.Where(x => x.LastName.ToLower().Replace(" ", "").Contains(LastName.ToLower().Replace(" ", "")));
            }

            if (!string.IsNullOrWhiteSpace(PhoneNumber))
            {
                query = query.Where(x =>
                    x.PhoneNumber.ToLower().Replace(" ", "").Contains(PhoneNumber.ToLower().Replace(" ", "")));
            }

            if (RegionIds != null && RegionIds.Any())
            {
                query = query.Where(x => RegionIds.Contains(x.RegionId));
            }

            return query;
        }
    }
}
