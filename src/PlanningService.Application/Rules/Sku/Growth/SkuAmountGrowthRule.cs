using PlanningService.Application.Contracts.Planner.Enums;
using PlanningService.Application.Interfaces;
using PlanningService.Application.Models;
using ValueType = PlanningService.Application.Contracts.Planner.Enums.ValueType;

namespace PlanningService.Application.Rules.Sku.Growth;

public class SkuAmountGrowthRule : IFormulaRule
{
    public bool CanApply(ICalculationNode node, ValueType valueType, Column column)
        => node is SkuNode && valueType is ValueType.AMOUNT && column is Column.ContributionGrowth;

    public void Apply(ICalculationNode node, ValueType valueType, ICalculationContext context)
    {
        var skuNode = (SkuNode)node;

        if (context.Total is null || context.Total.AmountHistory <= 0)
        {
            return;
        }

        skuNode.AmountGrowth = (skuNode.AmountPlanning - skuNode.AmountHistory) / context.Total.AmountHistory;
    }
}