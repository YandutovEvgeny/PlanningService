using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PlanningService.Domain.Entities;

/// <summary>
/// Represents the SKUSub table.<br/>
/// Subordinate entity.
/// </summary>
[Table("SKUSub")]
public class SkuSub : EntityBase
{
    /// <summary>
    /// Represents unique identifire of SKUSub.
    /// </summary>
    [Column("SKUSubId")]
    public override Guid Id { get; set; }

    /// <summary>
    /// Represents the parent SKU identifire.
    /// </summary>
    [Column("SKUId")]
    public required Guid SkuId { get; set; }

    /// <summary>
    /// Represents the name of SKUSub.
    /// </summary>
    [Column("SKUSubName")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Represents the SKU price.
    /// </summary>
    [Column("SKUPRICE")]
    public decimal Price { get; set; }

    /// <summary>
    /// Represents the SKU ratio.
    /// </summary>
    [Column("SKURatio")]
    public decimal Ratio { get; set; }

    /// <summary>
    /// Navigation property of the Sku.
    /// </summary>
    [AllowNull]
    public Sku Sku { get; set; }

    /// <summary>
    /// Represents the child history members.
    /// </summary>
    [AllowNull]
    public ICollection<HistoryY0> HistoryMembers { get; set; }

    /// <summary>
    /// Represents the child planning members.
    /// </summary>
    [AllowNull]
    public ICollection<PlanningY1> PlanningMembers { get; set; }
}
