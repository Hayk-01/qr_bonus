using InnLine.BLL.Filters;
using QRBonus.DAL.Models;
using QRBonus.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.BLL.Filters
{
    public class CampaignFilter : BaseFilter<Campaign>
    {
        public long? Id { get; set; }
        public string? Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public List<long>? CampaignStatusIds { get; set; }
        public List<long>? CampaignIds { get; set; }
        public List<long>? RegionIds { get; set; }

        public override IQueryable<Campaign> CreateQuery(IQueryable<Campaign> query)
        {
            if (Id.HasValue)
            {
                query = query.Where(x => x.Id == Id.Value);
            }

            if (CampaignStatusIds != null && CampaignStatusIds.Any())
            {
                query = query.Where(x => CampaignStatusIds.Contains((long)x.Status));
            }

            if (!string.IsNullOrWhiteSpace(Name))
            {
                query = query.Where(x =>
                    x.Translations.Any(x => x.Name.ToLower().Replace(" ", "").Contains(Name.ToLower().Replace(" ", ""))));
            }

            if (StartDate.HasValue)
            {
                query = query.Where(x => x.StartDate >= StartDate);
            }

            if (EndDate.HasValue)
            {
                query = query.Where(x => x.EndDate <= EndDate);
            }

            if (CampaignIds != null && CampaignIds.Any())
            {
                query = query.Where(x => CampaignIds.Contains(x.Id));
            }  
            
            if (RegionIds != null && RegionIds.Any())
            {
                query = query.Where(x => RegionIds.Contains(x.RegionId));
            }

            return query.OrderByDescending(x => x.Id);
        }
    } 
}