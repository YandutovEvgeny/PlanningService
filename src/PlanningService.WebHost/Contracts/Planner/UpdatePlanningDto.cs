namespace PlanningService.WebHost.Contracts.Planner;

public record class UpdatePlanningDto
{
    public required Guid SkuSubId { get; set; }
    public decimal Units { get; set; }
    public decimal Amount { get; set; }
}
