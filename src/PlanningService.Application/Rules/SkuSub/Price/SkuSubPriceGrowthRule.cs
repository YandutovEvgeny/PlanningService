using PlanningService.Application.Contracts.Planner.Enums;
using PlanningService.Application.Interfaces;
using PlanningService.Application.Models;
using ValueType = PlanningService.Application.Contracts.Planner.Enums.ValueType;

namespace PlanningService.Application.Rules.SkuSub.Price;

public class SkuSubPriceGrowthRule : IFormulaRule
{
    public bool CanApply(ICalculationNode node, ValueType valueType, Column column)
        => node is SkuSubNode && valueType is ValueType.PRICE && column is Column.ContributionGrowth;

    public void Apply(ICalculationNode node, ValueType valueType, ICalculationContext context)
    {
        var skuSubNode = (SkuSubNode)node;

        var parent = skuSubNode.ParentNode;
        if (parent is null) return;

        skuSubNode.PriceGrowth = (skuSubNode.PricePlanning - skuSubNode.PriceHistory) / parent.PricePlanning;
    }
}