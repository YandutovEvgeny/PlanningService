using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlanningService.Domain.Entities;
using PlanningService.Infrastructure.Extensions;

namespace PlanningService.Infrastructure.Configurations;

public class SkuConfiguration : IEntityTypeConfiguration<Sku>
{
    public void Configure(EntityTypeBuilder<Sku> builder)
    {
        builder.ConfigureBaseEntity();

        builder.Property(s => s.Name)
            .HasMaxLength(50);

        builder.HasMany(s => s.SubItems)
            .WithOne(ss => ss.Sku)
            .HasForeignKey(ss => ss.SkuId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(
            CreateSku(new Guid("4EFDD53A-3E63-476A-8B05-64B23F7B1677"), "Напитки"),
            CreateSku(new Guid("9F820331-78F3-4255-B15A-8FCB33E8BF6E"), "Фрукты"),
            CreateSku(new Guid("968FE5FE-90D9-42D8-BB26-B437E359230A"), "Овощи")
        );
    }

    private static Sku CreateSku(Guid id, string name)
    {
        return new Sku
        {
            Id = id,
            Name = name
        };
    }
}
