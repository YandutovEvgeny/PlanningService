using PlanningService.Application.Engine;
using PlanningService.Application.Interfaces;
using PlanningService.Application.Services.PlannerService;
using PlanningService.Domain.Interfaces;
using PlanningService.Infrastructure.Repositories;
using PlanningService.Application.Contracts.Planner.Enums;
using PlanningService.Application.Rules.SkuSub.Price;
using PlanningService.Application.Rules.SkuSub.Amount;
using PlanningService.Application.Rules.SkuSub.Units;
using PlanningService.Application.Rules.Sku.Amount;
using PlanningService.Application.Rules.Sku.Units;
using PlanningService.Application.Rules.Sku.Price;
using PlanningService.Application.Rules.Total.Amount;
using PlanningService.Application.Rules.Total.Units;
using PlanningService.Application.Rules.Total.Price;

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
            builder.For(Level.SkuSub, Column.PlanningY1)
                .AddRule<SkuSubPricePlanningY1Rule>()
                .AddRule<SkuSubAmountPlanningY1Rule>();

            builder.For(Level.SkuSub, Column.HistoryY0)
                .AddRule<SkuSubAmountHistoryY0Rule>()
                .AddRule<SkuSubPriceHistoryY0Rule>();

            builder.For(Level.SkuSub, Column.ContributionGrowth)
                .AddRule<SkuSubAmountGrowthRule>()
                .AddRule<SkuSubPriceGrowthRule>()
                .AddRule<SkuSubUnitsGrowthRule>();

            builder.For(Level.Sku, Column.PlanningY1)
                .AddRule<SkuAmountPlanningY1Rule>()
                .AddRule<SkuUnitsPlanningY1Rule>()
                .AddRule<SkuPricePlanningY1Rule>();

            builder.For(Level.Sku, Column.HistoryY0)
                .AddRule<SkuAmountHistoryY0Rule>()
                .AddRule<SkuUnitsHistoryY0Rule>()
                .AddRule<SkuPriceHistoryY0Rule>();

            builder.For(Level.Sku, Column.ContributionGrowth)
                .AddRule<SkuAmountGrowthRule>()
                .AddRule<SkuUnitsGrowthRule>()
                .AddRule<SkuPriceGrowthRule>();

            builder.For(Level.Total, Column.PlanningY1)
                .AddRule<TotalAmountPlanningY1Rule>()
                .AddRule<TotalUnitsPlanningY1Rule>()
                .AddRule<TotalPricePlanningY1Rule>();

            builder.For(Level.Total, Column.HistoryY0)
                .AddRule<TotalAmountHistoryY0Rule>()
                .AddRule<TotalUnitsHistoryY0Rule>()
                .AddRule<TotalPriceHistoryY0Rule>();

            builder.For(Level.Total, Column.ContributionGrowth)
                .AddRule<TotalAmountGrowthRule>()
                .AddRule<TotalUnitsGrowthRule>()
                .AddRule<TotalPriceGrowthRule>();
        });

        return services;
    }
}