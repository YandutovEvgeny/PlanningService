using PlanningService.Application.Contracts.Planner.Enums;
using PlanningService.Application.Interfaces;
using PlanningService.Application.Models;
using ValueType = PlanningService.Application.Contracts.Planner.Enums.ValueType;

namespace PlanningService.Application.Rules.Total.Units;

public class TotalUnitsHistoryY0Rule : IFormulaRule
{
    public bool CanApply(ICalculationNode node, ValueType valueType, Column column)
        => node is TotalNode && valueType is ValueType.UNITS && column is Column.HistoryY0;

    public void Apply(ICalculationNode node, ValueType valueType, ICalculationContext context)
    {
        var totalNode = (TotalNode)node;

        totalNode.UnitsHistory = context.Skus.Sum(s => s.UnitsHistory);
    }
}