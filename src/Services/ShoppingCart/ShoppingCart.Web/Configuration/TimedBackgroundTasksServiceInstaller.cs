using Ecommerce.Common.Web;
using Quartz;
using ShoppingCart.Application.Services;

namespace ShoppingCart.Web.Configuration;

public class TimedBackgroundTasksServiceInstaller : IServiceInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionScopedJobFactory();
            var jobKey = new JobKey("TimeOutShoppingCartsJob");
            q.AddJob<TimeOutShoppingCartsJob>(opts => opts.WithIdentity(jobKey));
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("TimeOutShoppingCartsJobTrigger")
                .WithCronSchedule("0 */30 * ? * *"));
        });
        services.AddQuartzHostedService(opt => opt.WaitForJobsToComplete = true);
    }
}