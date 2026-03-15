namespace PlanningService.Application.Contracts.Planner;

public record class PlannerResponse
{
    public required List<PlannerRow> Data { get; set; }
    public required List<ColumnMetadata> Metadata { get; set; }
}
