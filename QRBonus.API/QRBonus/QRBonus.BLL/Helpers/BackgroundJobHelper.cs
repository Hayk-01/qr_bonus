using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace QRBonus.BLL.Helpers;

public static class BackgroundJobHelper
{
    public static async Task AddBackgroundJobs(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var recurringJobService = app.Services.GetRequiredService<IRecurringJobManager>();

        //recurringJobService.AddOrUpdate<IProductService>("SyncProducts",
        //    productService => productService.SyncProductsWithEmark(), Cron.Daily);
    }
}