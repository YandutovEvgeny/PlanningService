using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlanningService.Domain.Entities;
using PlanningService.Infrastructure.Extensions;

namespace PlanningService.Infrastructure.Configurations;

public class SkuSubConfiguration : IEntityTypeConfiguration<SkuSub>
{
    public void Configure(EntityTypeBuilder<SkuSub> builder)
    {
        builder.ConfigureBaseEntity();

        builder.Property(ss => ss.Name)
            .HasMaxLength(50);

        builder.HasIndex(ss => ss.SkuId);

        builder.HasOne(ss => ss.HistoryMember)
            .WithOne(h => h.SkuSub)
            .HasForeignKey<HistoryY0>(h => h.SkuSubId);

        builder.HasOne(ss => ss.PlanningMember)
            .WithOne(p => p.SkuSub)
            .HasForeignKey<PlanningY1>(p => p.SkuSubId);

        builder.HasData(
            CreateSkuSub(new Guid("FA5F1C30-462B-4ECF-AFAD-CA15F56D4782"), Guid.Parse("4EFDD53A-3E63-476A-8B05-64B23F7B1677"), "Кола 0.5л", 100m, 0m),
            CreateSkuSub(new Guid("0C6CA973-A664-44B3-87EE-10ED8F4BEF19"), Guid.Parse("9F820331-78F3-4255-B15A-8FCB33E8BF6E"), "Бананы 1кг", 133m, 0m),
            CreateSkuSub(new Guid("870A6EF2-52E7-4BC3-9B1D-B62063AE824A"), Guid.Parse("968FE5FE-90D9-42D8-BB26-B437E359230A"), "Помидоры 5кг", 350m, 0m)
        );
    }

    private static SkuSub CreateSkuSub(Guid id, Guid skuId, string name, decimal price, decimal ratio)
    {
        return new SkuSub
        {
            Id = id,
            SkuId = skuId,
            Name = name,
            Price = price,
            Ratio = ratio
        };
    }
}
