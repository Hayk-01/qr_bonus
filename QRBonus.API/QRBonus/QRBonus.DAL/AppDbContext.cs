using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using QRBonus.DAL.Models;
using QRBonus.DAL.Seeders;
using System.Linq.Expressions;

namespace QRBonus.DAL;
public class AppDbContext : DbContext
{
    private readonly LanguageConfiguration _languageConfiguration;
    private readonly IRegionConfiguration _regionConfiguration;

    public AppDbContext(DbContextOptions options, LanguageConfiguration languageConfiguration, IRegionConfiguration regionConfiguration) : base(options)
    {
        _languageConfiguration = languageConfiguration;
        _regionConfiguration = regionConfiguration;
    }

    public DbSet<Error> Errors { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductTranslation> ProductTranslations { get; set; }
    public DbSet<Campaign> Campaigns { get; set; }
    public DbSet<CampaignTranslation> CampaignTranslations { get; set; }
    public DbSet<ProductCampaign> ProductCampaigns { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerSession> CustomerSessions { get; set; }
    public DbSet<Prize> Prizes { get; set; }
    public DbSet<PrizeTranslation> PrizeTranslations { get; set; }
    public DbSet<QrCode> QrCodes { get; set; }
    public DbSet<Banner> Banners { get; set; }
    public DbSet<BannerTranslation> BannerTranslations { get; set; }
    public DbSet<CampaignPrize> CampaignPrizes { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<RegionTranslation> RegionTranslations { get; set; }

    public override int SaveChanges()
    {
        AddModificationDate();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AddModificationDate();
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);


        foreach (var property in modelBuilder.Model.GetEntityTypes()
        .SelectMany(t => t.GetProperties())
        .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetColumnType("decimal(18,2)");
        }

        modelBuilder.ApplyGlobalFilters<BaseTranslationEntity>(e =>
          e.IsDeleted == false && e.LanguageId == _languageConfiguration.LanguageId);

        if (_regionConfiguration != null)
        {
            modelBuilder.ApplyGlobalFilters<BaseWithRegionEntity>(e => e.IsDeleted == false && (e.RegionId == _regionConfiguration.RegionId));
        }

        modelBuilder.ApplyGlobalFilters<BaseEntity>(e => e.IsDeleted == false);

        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        modelBuilder.SeedData();
    }

    private void AddModificationDate()
    {
        var entries = ChangeTracker
                        .Entries()
                            .Where(e => e.Entity is BaseEntity && (
                                        e.State == EntityState.Added
                                || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            ((BaseEntity)entityEntry.Entity).ModifyDate = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Added)
            {
                ((BaseEntity)entityEntry.Entity).CreatedDate = DateTime.UtcNow;
            }
        }
    }
}

public static class ModelBuilderExtension
{
    public static void ApplyGlobalFilters<TInterface>(this ModelBuilder modelBuilder, Expression<Func<TInterface, bool>> expression)
    {
        var entities = modelBuilder.Model
            .GetEntityTypes()
            .Where(x => x.ClrType.BaseType == typeof(TInterface))
            .Select(e => e.ClrType);
        foreach (var entity in entities)
        {
            var newParam = Expression.Parameter(entity);
            var newbody = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), newParam, expression.Body);
            modelBuilder.Entity(entity).HasQueryFilter(Expression.Lambda(newbody, newParam));
        }
    }
}