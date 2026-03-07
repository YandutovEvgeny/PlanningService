namespace PlanningService.WebHost.Contracts.Planner;

public record class ModelQuery
{
    public string[]? SkuSubNames { get; set; }
    public string[]? Levels { get; set; }
}
