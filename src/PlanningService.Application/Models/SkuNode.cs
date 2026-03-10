using System.Diagnostics.CodeAnalysis;

namespace PlanningService.Application.Models;

/// <summary>
/// Represents the SKU level calculated node.
/// </summary>
public class SkuNode : CalculationNodeBase
{
    /// <inheritdoc/>
    public override string Level => "SKU";

    /// <summary>
    /// Represents unique identifire of SKU.
    /// </summary>
    public Guid SkuId { get; set; }

    /// <summary>
    /// Represents the name of SKU.
    /// </summary>
    public string SkuName { get; set; } = string.Empty;

    /// <summary>
    /// Represents the SKU's level childs.
    /// </summary>
    [AllowNull]
    public List<SkuSubNode> Childrens { get; set; }
}