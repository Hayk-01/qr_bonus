using InnLine.BLL.Filters;
using QRBonus.DAL.Models;

namespace QRBonus.BLL.Filters;
public class BannerFilter : BaseFilter<Banner>
{
    public long? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }

    public List<long>? BannerIds { get; set; }
    public List<long>? RegionIds { get; set; }

    public override IQueryable<Banner> CreateQuery(IQueryable<Banner> query)
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
        
        if (!string.IsNullOrWhiteSpace(Description))
        {
            query = query.Where(x =>
                x.Translations.Any(x => x.Description.ToLower().Replace(" ", "").Contains(Description.ToLower().Replace(" ", ""))));
        }

        if (BannerIds != null && BannerIds.Any())
        {
            query = query.Where(x => BannerIds.Contains(x.Id));
        }

        if (RegionIds != null && RegionIds.Any())
        {
            query = query.Where(x => RegionIds.Contains(x.RegionId));
        }

        return query.OrderByDescending(x => x.Id);
    }
}