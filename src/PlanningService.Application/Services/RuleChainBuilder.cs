using PlanningService.Application.Contracts.Planner.Enums;
using PlanningService.Application.Interfaces;

namespace PlanningService.Application.Services;

public class RuleChainBuilder
{
    private readonly Dictionary<(Level level, Column column), List<Type>> _chains = [];

    public RuleChainBuilder For(Level level, Column column)
    {
        var key = (level, column);
        if (!_chains.ContainsKey(key))
        {
            _chains[key] = [];
        }

        return this;
    }

    public RuleChainBuilder AddRule<T>() where T : IFormulaRule
    {
        var key = _chains.Last().Key;
        _chains[key].Add(typeof(T));
        return this;
    }

    public IRuleChainProvider Build()
    {
        return new RuleChainProvider(_chains);
    }
}