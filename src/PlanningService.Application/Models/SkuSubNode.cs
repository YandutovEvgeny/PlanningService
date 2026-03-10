namespace PlanningService.Application.Models;

/// <summary>
/// Represents the SKUSUB level calculated node.
/// </summary>
public class SkuSubNode : CalculationNodeBase
{
    /// <inheritdoc/>
    public override string Level => "SKUSUB";

    /// <summary>
    /// Represents the parent SKU node.
    /// </summary>
    public required SkuNode ParentNode { get; set; }

    /// <summary>
    /// Represents unique identifire of SKUSub.
    /// </summary>
    public Guid SkuSubId { get; set; }

    /// <summary>
    /// Represents the name of SKUSub.
    /// </summary>
    public string SkuSubName { get; set; } = string.Empty;

    /// <summary>
    /// Represents the SKU price.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Represents the SKU ratio.
    /// </summary>
    public decimal Ratio { get; set; }
}