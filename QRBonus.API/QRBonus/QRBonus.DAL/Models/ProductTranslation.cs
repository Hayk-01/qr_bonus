namespace QRBonus.DAL.Models
{
    public class ProductTranslation : BaseTranslationEntity
    {
        public long ProductId { get; set; }
        public string? Name { get; set; }
        public Product? Product { get; set; }
    }
}
