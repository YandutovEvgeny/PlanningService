using PlanningService.Application.Contracts.Planner.Enums;
using PlanningService.Application.Interfaces;
using PlanningService.Application.Models;
using ValueType = PlanningService.Application.Contracts.Planner.Enums.ValueType;

namespace PlanningService.Application.Rules.Sku.Price;

public class SkuPriceHistoryY0Rule : IFormulaRule
{
    public bool CanApply(ICalculationNode node, ValueType valueType, Column column)
        => node is SkuNode && valueType is ValueType.PRICE && column is Column.HistoryY0;

    public void Apply(ICalculationNode node, ValueType valueType, ICalculationContext context)
    {
        var skuNode = (SkuNode)node;

        skuNode.PriceHistory = skuNode.UnitsHistory > 0
            ? skuNode.AmountHistory / skuNode.UnitsHistory
            : default;
    }
}