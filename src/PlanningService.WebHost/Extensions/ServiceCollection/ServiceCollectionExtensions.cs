using PlanningService.Application.Engine;
using PlanningService.Application.Interfaces;
using PlanningService.Application.Services.PlannerService;
using PlanningService.Domain.Interfaces;
using PlanningService.Infrastructure.Repositories;
using PlanningService.Application.Contracts.Planner.Enums;
using PlanningService.Application.Rules.SkuSub.Price;
using PlanningService.Application.Rules.SkuSub.Amount;
using PlanningService.Application.Rules.SkuSub.Units;

namespace PlanningService.WebHost.Extensions.ServiceCollection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IPlannerRepository, PlannerRepository>();
        services.AddScoped<ICalculationEngine, CalculationEngine>();
        services.AddScoped<IPlannerService, PlannerService>();

        services.Scan(scan => scan
            .FromAssemblyOf<IFormulaRule>()
            .AddClasses(classes => classes.AssignableTo<IFormulaRule>())
            .AsImplementedInterfaces()
            .WithTransientLifetime()
        );

        services.AddRuleChains(builder =>
        {
            builder.For("SkuSub", Column.PlanningY1)
                .AddRule<SkuSubPricePlanningY1Rule>()
                .AddRule<SkuSubAmountPlanningY1Rule>();

            builder.For("SkuSub", Column.HistoryY0)
                .AddRule<SkuSubAmountHistoryY0Rule>()
                .AddRule<SkuSubPriceHistoryY0Rule>();

            builder.For("SkuSub", Column.ContributionGrowth)
                .AddRule<SkuSubAmountGrowthRule>()
                .AddRule<SkuSubPriceGrowthRule>()
                .AddRule<SkuSubUnitsGrowthRule>();
        });

        return services;
    }
}