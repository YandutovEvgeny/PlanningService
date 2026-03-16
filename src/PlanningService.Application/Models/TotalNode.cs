using PlanningService.Application.Contracts.Planner.Enums;

namespace PlanningService.Application.Models;

/// <summary>
/// Represents the total level calculated node.
/// </summary>
public class TotalNode : CalculationNodeBase
{
    /// <inheritdoc/>
    public override Level Level => Level.Total;
}