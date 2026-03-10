using PlanningService.Application.Contracts.Planner.Enums;
using PlanningService.Application.Interfaces;
using PlanningService.Application.Models;
using ValueType = PlanningService.Application.Contracts.Planner.Enums.ValueType;

namespace PlanningService.Application.Rules.Sku.Units;

public class SkuUnitsPlanningY1Rule : IFormulaRule
{
    public bool CanApply(ICalculationNode node, ValueType valueType, Column column)
        => node is SkuNode && valueType is ValueType.UNITS && column is Column.PlanningY1;

    public void Apply(ICalculationNode node, ValueType valueType, ICalculationContext context)
    {
        var skuNode = (SkuNode)node;

        skuNode.UnitsPlanning = skuNode.Childrens.Sum(c => c.UnitsPlanning * c.Ratio);
    }
}