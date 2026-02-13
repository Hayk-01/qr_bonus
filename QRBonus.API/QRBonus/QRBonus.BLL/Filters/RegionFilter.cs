using InnLine.BLL.Filters;
using QRBonus.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.BLL.Filters
{
    public class RegionFilter : BaseFilter<Region>
    {
        public long? Id { get; set; }
        public string? Name { get; set; }

        public override IQueryable<Region> CreateQuery(IQueryable<Region> query)
        {
            if (Id.HasValue)
            {
                query = query.Where(x => x.Id == Id.Value);
            }

            if (!string.IsNullOrWhiteSpace(Name))
            {
                query = query.Where(x =>
                    x.Translations.Any(x => x.Name.ToLower().Replace(" ", "").Contains(Name.ToLower().Replace(" ", ""))));
            }

            return query.OrderByDescending(x => x.Id);

        }
    }
}
