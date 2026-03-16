using PlanningService.Application.Contracts.Planner.Enums;

namespace PlanningService.Application.Contracts.Planner;

public record class AmountInfo
{
    public Guid MetadataId { get; set; }
    public Column Column { get; set; }
    public decimal Value { get; set; }
}