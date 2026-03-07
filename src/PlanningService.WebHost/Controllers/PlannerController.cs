using Microsoft.AspNetCore.Mvc;
using PlanningService.WebHost.Contracts.Planner;

namespace PlanningService.WebHost.Controllers;

/// <summary>
/// Planners's controller.
/// </summary>
[ApiController]
public class PlannerController : ControllerBase
{
    [HttpGet]
    public Task<ModelDto> GetPlanningModel([FromQuery] ModelQuery query)
    {
        throw new NotImplementedException();
    }

    [HttpPatch]
    public Task UpdatePlanningModel([FromBody] UpdatePlanningDto dto)
    {
        throw new NotImplementedException();
    }
}
