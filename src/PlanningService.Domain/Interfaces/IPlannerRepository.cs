using PlanningService.Domain.Entities;

namespace PlanningService.Domain.Interfaces;

public interface IPlannerRepository
{
    Task<List<Sku>> GetAllSkusAsync(CancellationToken cancellationToken);
    Task<List<SkuSub>> GetAllSkuSubsAsync(CancellationToken cancellationToken);
    Task<Dictionary<Guid, HistoryY0>> GetHistoryBySkuSubIdsAsync(IEnumerable<Guid> skuSubIds, CancellationToken cancellationToken);
    Task<Dictionary<Guid, PlanningY1>> GetPlanningBySkuSubIdsAsync(IEnumerable<Guid> skuSubIds, CancellationToken cancellationToken);
    Task UpdatePlanningAsync(Guid skuSubId, decimal newValue, CancellationToken cancellationToken);
}