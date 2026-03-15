using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PlanningService.Domain.Entities;

/// <summary>
/// Represents the SKU table.
/// </summary>
[Table("skus")]
public class Sku : EntityBase
{
    /// <summary>
    /// Represents unique identifire of SKU.
    /// </summary>
    [Column("sku_id")]
    public override Guid Id { get; set; }

    /// <summary>
    /// Represents the name of SKU.
    /// </summary>
    [Column("sku_name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Represents SKUSub items.
    /// </summary>
    [AllowNull]
    public ICollection<SkuSub> SubItems { get; set; }
}