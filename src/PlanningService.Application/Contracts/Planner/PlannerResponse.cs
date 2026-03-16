namespace PlanningService.Application.Contracts.Planner;

public record class PlannerResponse
{
    public required List<PlannerRow> Data { get; set; }
    public required List<MetadataModel> Metadata { get; set; }
}
