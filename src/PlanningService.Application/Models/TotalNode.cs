namespace PlanningService.Application.Models;

/// <summary>
/// Represents the total level calculated node.
/// </summary>
public class TotalNode : CalculationNodeBase
{
    /// <inheritdoc/>
    public override string Level => "Total";
}