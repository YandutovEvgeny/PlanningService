using PlanningService.Application.Engine;
using PlanningService.Application.Interfaces;
using PlanningService.Application.Services;
using PlanningService.Domain.Interfaces;
using PlanningService.Infrastructure.Repositories;

namespace PlanningService.WebHost.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<IPlannerRepository, PlannerRepository>();
        services.AddScoped<ICalculationEngine, CalculationEngine>();
        services.AddScoped<PlannerService>();

        services.Scan(scan => scan
            .FromAssemblyOf<IFormulaRule>()
            .AddClasses(classes => classes.AssignableTo<IFormulaRule>())
            .AsImplementedInterfaces()
            .WithTransientLifetime()
        );

        return services;
    }
}