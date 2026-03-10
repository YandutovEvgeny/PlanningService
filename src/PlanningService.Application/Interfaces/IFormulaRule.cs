using PlanningService.Application.Contracts.Planner.Enums;
using PlanningService.Application.Models;
using ValueType = PlanningService.Application.Contracts.Planner.Enums.ValueType;

namespace PlanningService.Application.Interfaces;

public interface IFormulaRule
{
    bool CanApply(ICalculationNode node, ValueType valueType, Column column);
    void Apply(ICalculationNode node, ValueType valueType, ICalculationContext context);
}