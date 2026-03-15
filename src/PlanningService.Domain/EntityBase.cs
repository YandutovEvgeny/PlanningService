using System.ComponentModel.DataAnnotations.Schema;

namespace PlanningService.Domain;

/// <summary>
/// Represents the base entity.
/// </summary>
public class EntityBase : IEntity
{
    /// <summary>
    /// Unique identifire if entity.
    /// </summary>
    [Column("id")]
    public virtual Guid Id { get; set; }
}
