using PlanningService.Application.Contracts.Planner.Enums;
using System.Diagnostics.CodeAnalysis;

namespace PlanningService.Application.Models;

/// <summary>
/// Represents the SKU level calculated node.
/// </summary>
public class SkuNode : CalculationNodeBase
{
    /// <inheritdoc/>
    public override Level Level => Level.Sku;

    /// <summary>
    /// Represents unique identifire of SKU.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Represents the name of SKU.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Represents the SKU's level childs.
    /// </summary>
    [AllowNull]
    public List<SkuSubNode> Childrens { get; set; }
}