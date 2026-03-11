using PlanningService.Application.Contracts.Planner.Enums;
using PlanningService.Application.Interfaces;
using PlanningService.Application.Models;
using ValueType = PlanningService.Application.Contracts.Planner.Enums.ValueType;

namespace PlanningService.Application.Rules.Total.Amount;

public class TotalAmountGrowthRule : IFormulaRule
{
    public bool CanApply(ICalculationNode node, ValueType valueType, Column column)
        => node is TotalNode && valueType is ValueType.AMOUNT && column is Column.ContributionGrowth;

    public void Apply(ICalculationNode node, ValueType valueType, ICalculationContext context)
    {
        var totalNode = (TotalNode)node;

        totalNode.AmountGrowth = (totalNode.AmountPlanning - totalNode.AmountHistory) / totalNode.AmountHistory;
    }
}