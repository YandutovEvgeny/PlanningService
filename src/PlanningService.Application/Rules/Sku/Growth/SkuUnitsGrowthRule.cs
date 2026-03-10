using PlanningService.Application.Contracts.Planner.Enums;
using PlanningService.Application.Interfaces;
using PlanningService.Application.Models;
using ValueType = PlanningService.Application.Contracts.Planner.Enums.ValueType;

namespace PlanningService.Application.Rules.Sku.Growth;

public class SkuUnitsGrowthRule : IFormulaRule
{
    public bool CanApply(ICalculationNode node, ValueType valueType, Column column)
        => node is SkuNode && valueType is ValueType.UNITS && column is Column.ContributionGrowth;

    public void Apply(ICalculationNode node, ValueType valueType, ICalculationContext context)
    {
        var skuNode = (SkuNode)node;

        if (context.Total is null || context.Total.UnitsHistory <= 0)
        {
            return;
        }

        skuNode.UnitsGrowth = (skuNode.UnitsPlanning - skuNode.UnitsHistory) / context.Total.UnitsHistory;
    }
}