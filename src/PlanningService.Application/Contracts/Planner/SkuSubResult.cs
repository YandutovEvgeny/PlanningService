namespace PlanningService.Application.Contracts.Planner;

public record class SkuSubResult
{
    public Guid SkuSubId { get; set; }
    public string SkuSubName { get; set; } = string.Empty;
    public Guid SkuId { get; set; }
    public string SkuName { get; set; } = string.Empty;
    public decimal Ratio { get; set; }

}