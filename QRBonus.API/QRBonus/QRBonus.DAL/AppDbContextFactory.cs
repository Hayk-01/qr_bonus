using Microsoft.EntityFrameworkCore;

namespace QRBonus.DAL;
public class AppDbContextFactory : DesignTimeDbContextFactory<AppDbContext>
{
    private readonly LanguageConfiguration _languageConfiguration;
    private readonly IRegionConfiguration _regionConfiguration;

    public AppDbContextFactory()
    {

    }

    public AppDbContextFactory(LanguageConfiguration languageConfiguration, IRegionConfiguration regionConfiguration)
    {
        _languageConfiguration = languageConfiguration;
        _regionConfiguration = regionConfiguration;
    }

    protected override AppDbContext CreateNewInstance(DbContextOptions<AppDbContext> options)
    {
        return new AppDbContext(options, _languageConfiguration, _regionConfiguration);
    }
}
