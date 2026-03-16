using PlanningService.Application.Contracts.Planner.Enums;

namespace PlanningService.Application.Contracts.Planner;

public record class PlannerFilter
{
    public string[]? SkuSubNames { get; set; }
    public Level Level { get; set; }
}