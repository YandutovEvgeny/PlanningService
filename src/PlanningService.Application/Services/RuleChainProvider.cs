using PlanningService.Application.Contracts.Planner.Enums;
using PlanningService.Application.Interfaces;

namespace PlanningService.Application.Services;

public class RuleChainProvider : IRuleChainProvider
{
    private readonly Dictionary<(string Level, Column Column), List<Type>> _chains;

    public RuleChainProvider(Dictionary<(string Level, Column Column), List<Type>> chains)
    {
        _chains = chains;
    }

    public IEnumerable<Type> GetRuleChain(string level, Column column)
    {
        return _chains.TryGetValue((level, column), out var chain)
            ? chain
            : Enumerable.Empty<Type>();
    }
}