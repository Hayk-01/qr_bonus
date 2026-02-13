using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace QRBonus.BLL.Services.TaskService;
public class TestExampleTaskManager : TaskManager
{
    public TestExampleTaskManager(ILogger<TaskManager> logger,
                                 IServiceProvider serviceProvider) : base(logger, serviceProvider)
    {
    }

    public override async Task ExecuteTasks(CancellationToken cancellationToken)
    {

        _logger.LogInformation("RSS Service running.");

        var disablePeriodic = new TimeSpan(0, 0, 0, 0, -1);

        //AddTask(async () =>
        //{
        //    using var scope = _serviceProvider.CreateScope();
        //    var service = scope.ServiceProvider.GetService<INewRSSService>();
        //    if (service == null)
        //        return;

        //    await service.RSSAsync();

        //}, TimeSpan.FromMinutes(1));

        await Task.CompletedTask;
    }
}
