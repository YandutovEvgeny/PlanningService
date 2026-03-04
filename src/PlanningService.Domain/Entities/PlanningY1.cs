using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PlanningService.Domain.Entities;

/// <summary>
/// Represents the PlanningY1 table.<br/>
/// Data for current period.
/// </summary>
[Table("PlanningY1")]
public class PlanningY1 : EntityBase
{
    /// <summary>
    /// Represents the SKUSub unique identifire.
    /// </summary>
    [Column("SKUSubId")]
    public Guid SkuSubId { get; set; }

    /// <summary>
    /// Represents the SKUSub units.
    /// </summary>
    [Column("Units")]
    public decimal Units { get; set; }

    /// <summary>
    /// Represents the SKUSub amount.
    /// </summary>
    [Column("Amount")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Navigation property of the SkuSub.
    /// </summary>
    [AllowNull]
    public SkuSub SkuSub { get; set; }
}
