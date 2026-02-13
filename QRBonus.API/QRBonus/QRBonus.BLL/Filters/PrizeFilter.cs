using InnLine.BLL.Filters;
using QRBonus.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.BLL.Filters
{
    public class PrizeFilter : BaseFilter<Prize>
    {
        public long? Id { get; set; }
        public string? Name { get; set; }
        public bool? IsLeaderboard { get; set; }
        public List<long>? RegionIds { get; set; }
        public override IQueryable<Prize> CreateQuery(IQueryable<Prize> query)
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

            if (IsLeaderboard.HasValue)
            {
                query = query.Where(x => x.IsLeaderboard == IsLeaderboard.Value);
            }

            if (RegionIds != null && RegionIds.Any())
            {
                query = query.Where(x => RegionIds.Contains(x.RegionId));
            }

            return query.OrderByDescending(x => x.Id);
        }
    }
}
