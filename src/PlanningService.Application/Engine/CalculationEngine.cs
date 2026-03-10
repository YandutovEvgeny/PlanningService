using PlanningService.Application.Interfaces;
using PlanningService.Application.Models;
using PlanningService.Application.Contracts.Planner.Enums;
using ValueType = PlanningService.Application.Contracts.Planner.Enums.ValueType;

namespace PlanningService.Application.Engine;

public class CalculationEngine : ICalculationEngine
{
    private readonly IEnumerable<IFormulaRule> _rules;

    public CalculationEngine(IEnumerable<IFormulaRule> rules)
    {
        _rules = rules;
    }

    public void Calculate(ICalculationContext context)
    {
        //For SkuSub
        ApplyToNodes(context.Skus.SelectMany(s => s.Childrens), context);

        //For Sku
        ApplyToNodes(context.Skus, context);

        //For Total
        if (context.Total != null)
        {
            ApplyToNodes([context.Total], context);
        }
    }

    private void ApplyToNodes(IEnumerable<ICalculationNode> nodes, ICalculationContext context)
    {
        foreach (var node in nodes)
        {
            foreach (Column column in Enum.GetValues(typeof(Column)))
            {
                foreach (ValueType type in Enum.GetValues(typeof(ValueType)))
                {
                    foreach (var rule in _rules.Where(r => r.CanApply(node, type, column)))
                    {
                        rule.Apply(node, type, context);
                    }
                }
            }
        }
    }
}