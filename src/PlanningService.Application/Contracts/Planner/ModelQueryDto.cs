namespace PlanningService.WebHost.Contracts.Planner;

public record class ModelQueryDto
{
    public string[]? SkuSubNames { get; set; }
    public string[]? Levels { get; set; }
}
