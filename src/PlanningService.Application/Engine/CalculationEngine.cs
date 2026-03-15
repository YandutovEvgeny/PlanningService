using PlanningService.Application.Interfaces;
using PlanningService.Application.Models;
using PlanningService.Application.Contracts.Planner.Enums;
using ValueType = PlanningService.Application.Contracts.Planner.Enums.ValueType;

namespace PlanningService.Application.Engine;

public class CalculationEngine : ICalculationEngine
{
    private readonly Dictionary<Type, IFormulaRule> _rules;
    private readonly IRuleChainProvider _ruleChainProvider;

    public CalculationEngine(IEnumerable<IFormulaRule> rules, IRuleChainProvider ruleChainProvider)
    {
        _rules = rules.ToDictionary(r => r.GetType());
        _ruleChainProvider = ruleChainProvider;
    }

    public void Calculate(ICalculationContext context)
    {
        ApplyToNodes(context);
    }

    private void ApplyToNodes(ICalculationContext context)
    {
        var allNodes = GetAllNodes(context);

        foreach(var node in allNodes)
        {
            foreach (Column column in Enum.GetValues(typeof(Column)))
            {
                foreach (ValueType type in Enum.GetValues(typeof(ValueType)))
                {
                    var chain = _ruleChainProvider.GetRuleChain(node.Level, column);
                    foreach (var ruleType in chain)
                    {
                        if (_rules.TryGetValue(ruleType, out var rule) && rule.CanApply(node, type, column))
                        {
                            rule.Apply(node, type, context);
                        }
                    }
                }
            }
        }
    }

    private IEnumerable<ICalculationNode> GetAllNodes(ICalculationContext context)
    {
        yield return context.Total!;
        foreach (var sku in context.Skus)
        {
            yield return sku;
            foreach (var child in sku.Childrens)
            {
                yield return child;
            }
        }
    }
}