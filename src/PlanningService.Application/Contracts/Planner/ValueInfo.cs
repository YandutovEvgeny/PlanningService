using ValueType = PlanningService.Application.Contracts.Planner.Enums.ValueType;

namespace PlanningService.Application.Contracts.Planner;

public class ValueInfo
{
    public ValueType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Value { get; set; }
}