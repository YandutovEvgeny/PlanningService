namespace PlanningService.Application.Contracts.Planner;

public record class PlannerFilter
{
    public string[]? SkuSubNames { get; set; }
    public string[]? Levels { get; set; }
}