using PlanningService.Application.Contracts.Planner.Enums;
using PlanningService.Application.Interfaces;
using PlanningService.Application.Models;
using ValueType = PlanningService.Application.Contracts.Planner.Enums.ValueType;

namespace PlanningService.Application.Rules.Total.Price;

public class TotalPriceHistoryY0Rule : IFormulaRule
{
    public bool CanApply(ICalculationNode node, ValueType valueType, Column column)
        => node is TotalNode && valueType is ValueType.PRICE && column is Column.HistoryY0;

    public void Apply(ICalculationNode node, ValueType valueType, ICalculationContext context)
    {
        var totalNode = (TotalNode)node;

        totalNode.PriceHistory = totalNode.UnitsHistory > 0
            ? totalNode.AmountHistory / totalNode.UnitsHistory
            : default;
    }
}