using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlanningService.Domain;

namespace PlanningService.Infrastructure.Extensions;

public static class BaseEntityConfigurationExtension
{
    public static void ConfigureBaseEntity<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : EntityBase
    {
        builder.HasKey(x => x.Id);
    }
}