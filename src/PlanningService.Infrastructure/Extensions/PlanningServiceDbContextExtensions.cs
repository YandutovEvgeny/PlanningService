using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PlanningService.Infrastructure.Options;
using Microsoft.Extensions.Logging;

namespace PlanningService.Infrastructure.Extensions;

public static class PlanningServiceDbContextExtensions
{
    public static IServiceCollection AddPlanningServiceDbContext(this IServiceCollection services, Action<PlanningServiceDbContextOptions> configure)
    {
        services.AddOptions<PlanningServiceDbContextOptions>()
            .Configure(configure)
            .ValidateOnStart();
        
        services.AddDbContext<PlannerDbContext>((sp, options) =>
        {
            var dbContextOptions = sp.GetRequiredService<IOptions<PlanningServiceDbContextOptions>>();

            options.UseSqlite(dbContextOptions.Value.Connection)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging();
            options.UseSnakeCaseNamingConvention();
        });

        return services;
    }
}
