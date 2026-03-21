using PlanningService.Application.Contracts.Planner.Enums;

namespace PlanningService.Application.Contracts.Planner;

public record class PlannerRow
{
    public Guid Id { get; set; }
    public Guid ParentId { get; set; }
    public Level Level { get; set; }
    public string Title { get; set; } = string.Empty;
    public List<PriceInfo> PriceInfos { get; set; } = [];
    public List<UnitsInfo> UnitsInfos { get; set; } = [];
    public List<AmountInfo> AmountInfos { get; set; } = [];
}