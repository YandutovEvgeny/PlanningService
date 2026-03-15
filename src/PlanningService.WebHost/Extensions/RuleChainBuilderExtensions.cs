using PlanningService.Application.Interfaces;
using PlanningService.Application.Services;

namespace PlanningService.WebHost.Extensions;

public static class RuleChainBuilderExtensions
{
    public static IServiceCollection AddRuleChains(this IServiceCollection services, Action<RuleChainBuilder> configure)
    {
        var builder = new RuleChainBuilder();
        configure(builder);
        services.AddSingleton<IRuleChainProvider>(builder.Build());
        return services;
    }
}