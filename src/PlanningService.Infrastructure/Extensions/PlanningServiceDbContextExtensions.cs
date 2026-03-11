using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PlanningService.Infrastructure.Options;

namespace PlanningService.Infrastructure.Extensions;

public static class PlanningServiceDbContextExtensions
{
    public static IServiceCollection AddPlanningServiceDbContext(this IServiceCollection services, Action<PlanningServiceDbContextOptions> configure)
    {
        services.AddOptions<PlanningServiceDbContextOptions>()
            .Configure(configure)
            .ValidateOnStart();
        
        services.AddDbContext<PlanningDbContext>((sp, options) =>
        {
            var dbContextOptions = sp.GetRequiredService<IOptions<PlanningServiceDbContextOptions>>();

            options.UseInMemoryDatabase(dbContextOptions.Value.DatabaseName!);
            options.UseSnakeCaseNamingConvention();
            options.EnableSensitiveDataLogging();
        });

        return services;
    }
}
