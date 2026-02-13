using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QRBonus.BLL.Helpers;
using QRBonus.BLL.Models;
using QRBonus.BLL.Services.BannerService;
using QRBonus.BLL.Services.CampaignService;
using QRBonus.BLL.Services.CustomerService;
using QRBonus.BLL.Services.ErrorService;
using QRBonus.BLL.Services.PrizeService;
using QRBonus.BLL.Services.ProductService;
using QRBonus.BLL.Services.PushNotificationService;
using QRBonus.BLL.Services.QrCodeService;
using QRBonus.BLL.Services.RegionService;
using QRBonus.BLL.Services.ReportService;
using QRBonus.BLL.Services.SmsService;
using QRBonus.BLL.Services.UserService;
using QRBonus.DAL;

namespace QRBonus.BLL;
public static class ServiceExtension
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<FileSettings>(options => configuration.GetSection(nameof(FileSettings)).Bind(options));
        services.Configure<TwilioSettings>(options => configuration.GetSection(nameof(TwilioSettings)).Bind(options));
        services.Configure<MsgGeSettings>(options => configuration.GetSection(nameof(MsgGeSettings)).Bind(options));
        services.Configure<NikitaSettings>(options => configuration.GetSection(nameof(NikitaSettings)).Bind(options));

        services.AddScoped<IErrorService, ErrorService>(); 
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICampaignService, CampaignService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ICustomerSessionService, CustomerSessionService>();
        services.AddKeyedScoped<ISmsService, TwilioSmsService>("twilio");
        services.AddKeyedScoped<ISmsService, NikitaSmsService>("nikita");
        services.AddKeyedScoped<ISmsService, MsgGeSmsService>("msgGe");
        services.AddScoped<IPrizeService, PrizeService>();
        services.AddScoped<IBannerService, BannerService>();
        services.AddScoped<IQrCodeService, QrCodeService>();
        services.AddScoped<IRegionService, RegionService>();
        services.AddScoped<IPushNotificationService, PushNotificationService>();

        services.AddScoped<LanguageConfiguration>();
        services.AddScoped<IRegionConfiguration, RegionConfiguration>();

        services.AddHttpClient();
        services.AddHttpContextAccessor();

        return services;
    }

    public static IServiceCollection RegisterAdminServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<FileSettings>(options => configuration.GetSection(nameof(FileSettings)).Bind(options));
        services.Configure<MartinSettings>(options => configuration.GetSection(nameof(MartinSettings)).Bind(options));

        services.AddScoped<IErrorService, ErrorService>();
        services.AddScoped<IProductAdminService, ProductAdminService>();
        services.AddScoped<ICustomerAdminService, CustomerAdminService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserSessionService, UserSessionService>();
        services.AddScoped<IPrizeAdminService, PrizeAdminService>();
        services.AddScoped<IQrCodeAdminService, QrCodeAdminService>();
        services.AddScoped<ICampaignAdminService, CampaignAdminService>();
        services.AddScoped<IBannerAdminService, BannerAdminService>();
        services.AddScoped<IReportAdminService, ReportAdminService>();
        services.AddScoped<IRegionAdminService, RegionAdminService>();
        services.AddScoped<IRegionConfiguration>(x => null);
        services.AddScoped<IPushNotificationService, PushNotificationService>();

        services.AddScoped<LanguageConfiguration>();
        services.AddScoped<FileHelper>();

        services.AddHttpContextAccessor();

        return services;
    }
}
