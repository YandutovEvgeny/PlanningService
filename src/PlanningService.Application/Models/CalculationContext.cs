namespace PlanningService.Application.Models;

public class CalculationContext : ICalculationContext
{
    public TotalNode? Total { get; set; }
    public IEnumerable<SkuNode> Skus { get; set; } = [];
}