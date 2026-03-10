namespace PlanningService.Application.Models;

public interface ICalculationContext
{
    TotalNode? Total { get; }
    IEnumerable<SkuNode> Skus { get; }
}