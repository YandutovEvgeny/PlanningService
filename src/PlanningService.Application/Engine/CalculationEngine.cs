using PlanningService.Application.Interfaces;
using PlanningService.Application.Models;
using Column = PlanningService.Application.Contracts.Planner.Enums.Column;

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

        foreach (var node in allNodes)
        {
            foreach (Column column in Enum.GetValues(typeof(Column)))
            {
                var chain = _ruleChainProvider.GetRuleChain(node.Level, column);
                foreach (var ruleType in chain)
                {
                    if (_rules.TryGetValue(ruleType, out var rule))
                    {
                        rule.Apply(node, context);
                    }
                }
            }
        }
    }

    private IEnumerable<ICalculationNode> GetAllNodes(ICalculationContext context)
    {
        foreach (var skuSub in context.SkuSubs)
        {
            yield return skuSub;
        }

        if (context.Skus.Any())
        {
            foreach (var sku in context.Skus)
            {
                yield return sku;
            }
        }

        if (context.Total is not null)
        {
            yield return context.Total!;
        }
    }
}