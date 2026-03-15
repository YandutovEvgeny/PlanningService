using Microsoft.AspNetCore.Mvc;
using PlanningService.Application.Contracts;
using PlanningService.Application.Contracts.Planner;
using PlanningService.Application.Services.PlannerService;
using PlanningService.WebHost.Contracts.Planner;

namespace PlanningService.WebHost.Controllers;

/// <summary>
/// Planners's controller.
/// </summary>
[ApiController]
[Route("api/planner")]
public class PlannerController : ControllerBase
{
    private readonly IPlannerService _plannerService;

    public PlannerController(IPlannerService plannerService)
    {
        _plannerService = plannerService;
    }

    /// <summary>
    /// Returns calculated planning model.
    /// </summary>
    /// <param name="query">Represents the model which contained filters for calculation.</param>
    /// <returns>Calculated planning model.</returns>
    [HttpGet]
    public async Task<PlannerResponse> GetPlannerModel([FromQuery] ModelQueryDto query)
    {
        var filter = new PlannerFilter
        {
            Levels = query.Levels,
            SkuSubNames = query.SkuSubNames
        };

        return await _plannerService.GetPlannerDataAsync(filter);
    }

    /// <summary>
    /// Allows update planning model by sub sku identifire.
    /// </summary>
    /// <param name="skuSubId">Identifire sub sku object.</param>
    /// <param name="dto">Data for updating.</param>
    /// <returns>Updateing object identifire.</returns>
    [HttpPatch("{skuSubId:Guid}")]
    public async Task<ResponseId<Guid>> UpdatePlanningModel([FromQuery] Guid skuSubId, [FromBody] UpdatePlanningDto dto)
    {
       return await _plannerService.UpdatePlanningAsync(skuSubId, dto.Units);
    }
}
