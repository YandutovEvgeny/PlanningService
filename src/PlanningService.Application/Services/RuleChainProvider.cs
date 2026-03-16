using PlanningService.Application.Contracts.Planner.Enums;
using PlanningService.Application.Interfaces;

namespace PlanningService.Application.Services;

public class RuleChainProvider : IRuleChainProvider
{
    private readonly Dictionary<(Level Level, Column Column), List<Type>> _chains;

    public RuleChainProvider(Dictionary<(Level Level, Column Column), List<Type>> chains)
    {
        _chains = chains;
    }

    public IEnumerable<Type> GetRuleChain(Level level, Column column)
    {
        return _chains.TryGetValue((level, column), out var chain)
            ? chain
            : Enumerable.Empty<Type>();
    }
}