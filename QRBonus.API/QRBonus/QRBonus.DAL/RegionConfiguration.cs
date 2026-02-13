namespace QRBonus.DAL
{
    public interface IRegionConfiguration
    {
        public long RegionId { get; set; }
    }
    
    public class RegionConfiguration : IRegionConfiguration
    {
        public long RegionId { get; set; }
    }
}
