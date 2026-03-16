using PlanningService.Application.Contracts.Planner.Enums;

namespace PlanningService.Application.Interfaces;

public interface IRuleChainProvider
{
    IEnumerable<Type> GetRuleChain(Level level, Column column);
}