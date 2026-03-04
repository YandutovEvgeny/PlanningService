namespace PlanningService.Domain;

/// <summary>
/// Represents the base entity.
/// </summary>
public class EntityBase : IEntity
{
    /// <summary>
    /// Unique identifire if entity.
    /// </summary>
    public virtual Guid Id { get; set; }
}
