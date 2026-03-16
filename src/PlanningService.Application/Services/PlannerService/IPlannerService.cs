using PlanningService.Application.Contracts;
using PlanningService.Application.Contracts.Planner;

namespace PlanningService.Application.Services.PlannerService;

public interface IPlannerService
{
    Task<PlannerResponse> GetPlannerDataAsync(PlannerFilter filter, CancellationToken cancellationToken = default);
    Task<ResponseId<Guid>> UpdatePlanningAsync(Guid skuSubId, UpdatePlanningDto data, CancellationToken cancellationToken = default);
}