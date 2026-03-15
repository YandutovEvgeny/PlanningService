namespace PlanningService.Application.Contracts.Planner;

public record class PlannerRow
{
    public required string Level { get; set; }
    public string Title { get; set; } = string.Empty;
    public required ValueInfo ValueInfo { get; set; }
}