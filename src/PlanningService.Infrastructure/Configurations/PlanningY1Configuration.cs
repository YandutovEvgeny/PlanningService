using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlanningService.Domain.Entities;
using PlanningService.Infrastructure.Extensions;

namespace PlanningService.Infrastructure.Configurations;

public class PlanningY1Configuration : IEntityTypeConfiguration<PlanningY1>
{
    public void Configure(EntityTypeBuilder<PlanningY1> builder)
    {
        builder.ConfigureBaseEntity();

        builder.HasIndex(p => p.SkuSubId);

        builder.HasData(
            CreatePlanningY1(new Guid("1890B531-361C-4A33-BDAD-BD739762ED46"), Guid.Parse("FA5F1C30-462B-4ECF-AFAD-CA15F56D4782"), 0m, 0m),
            CreatePlanningY1(new Guid("55E06EE5-23E8-4895-98DF-71888EC2A0CC"), Guid.Parse("0C6CA973-A664-44B3-87EE-10ED8F4BEF19"), 0m, 0m),
            CreatePlanningY1(new Guid("226A097E-A260-4168-99D1-36B5B163EFFC"), Guid.Parse("870A6EF2-52E7-4BC3-9B1D-B62063AE824A"), 0m, 0m)
        );
    }

    private static PlanningY1 CreatePlanningY1(Guid id, Guid skuSubId, decimal amount, decimal units)
    {
        return new PlanningY1
        {
            Id = id,
            SkuSubId = skuSubId,
            Amount = amount,
            Units = units
        };
    }
}
