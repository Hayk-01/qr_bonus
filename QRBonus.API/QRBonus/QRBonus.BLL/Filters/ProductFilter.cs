using InnLine.BLL.Filters;
using QRBonus.DAL.Models;
namespace QRBonus.BLL.Filters
{
    public class ProductFilter : BaseFilter<Product>
    {
        public long? Id { get; set; }
        public string? Name { get; set; }
        public List<long>? RegionIds { get; set; }

        public override IQueryable<Product> CreateQuery(IQueryable<Product> query)
        {
            if (Id.HasValue)
            {
                query = query.Where(x => x.Id == Id.Value);
            }

            if (!string.IsNullOrWhiteSpace(Name))
            {
                return query
                    .Where(x => x.Translations.Any(y => y.Name.ToLower().Replace(" ", "").Contains(Name.ToLower().Replace(" ", ""))));
            }

            if (RegionIds != null && RegionIds.Any())
            {
                query = query.Where(x => RegionIds.Contains(x.RegionId));
            }

            return query.OrderByDescending(x => x.Id);
        }
    }
}
