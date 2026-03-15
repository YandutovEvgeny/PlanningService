using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PlanningService.Domain.Entities;

/// <summary>
/// Represents the SKUSub table.<br/>
/// Subordinate entity.
/// </summary>
[Table("skusub")]
public class SkuSub : EntityBase
{
    /// <summary>
    /// Represents unique identifire of SKUSub.
    /// </summary>
    [Column("id")]
    public override Guid Id { get; set; }

    /// <summary>
    /// Represents the parent SKU identifire.
    /// </summary>
    [Column("sku_id")]
    public required Guid SkuId { get; set; }

    /// <summary>
    /// Represents the name of SKUSub.
    /// </summary>
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Represents the SKU price.
    /// </summary>
    [Column("sku_price")]
    public decimal Price { get; set; }

    /// <summary>
    /// Represents the SKU ratio.
    /// </summary>
    [Column("sku_ratio")]
    public decimal Ratio { get; set; }

    /// <summary>
    /// Navigation property of the Sku.
    /// </summary>
    [AllowNull]
    public Sku Sku { get; set; }

    /// <summary>
    /// Represents the child history member.
    /// </summary>
    [AllowNull]
    public HistoryY0 HistoryMember { get; set; }

    /// <summary>
    /// Represents the child planning member.
    /// </summary>
    [AllowNull]
    public PlanningY1 PlanningMember { get; set; }
}
