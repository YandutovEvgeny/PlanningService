using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PlanningService.Domain.Entities;

/// <summary>
/// Represents the PlanningY1 table.<br/>
/// Data for current period.
/// </summary>
[Table("planningY1")]
public class PlanningY1 : EntityBase
{
    /// <summary>
    /// Represents the SKUSub unique identifire.
    /// </summary>
    [Column("skusub_id")]
    public Guid SkuSubId { get; set; }

    /// <summary>
    /// Represents the SKUSub units.
    /// </summary>
    [Column("units")]
    public decimal Units { get; set; }

    /// <summary>
    /// Represents the SKUSub amount.
    /// </summary>
    [Column("amount")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Navigation property of the SkuSub.
    /// </summary>
    [AllowNull]
    public SkuSub SkuSub { get; set; }
}
