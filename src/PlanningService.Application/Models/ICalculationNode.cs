namespace PlanningService.Application.Models;

/// <summary>
/// Represents the calculation node.
/// </summary>
public interface ICalculationNode
{
    /// <summary>
    /// Represents the node's level.<br/>
    /// <b>Example</b>: Total, SKU, SKUSUB or etc.
    /// </summary>
    string Level { get; }


}