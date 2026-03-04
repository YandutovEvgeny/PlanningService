using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlanningService.Domain.Entities;
using PlanningService.Infrastructure.Extensions;

namespace PlanningService.Infrastructure.Configurations;

public class HistoryY0Configuration : IEntityTypeConfiguration<HistoryY0>
{
    public void Configure(EntityTypeBuilder<HistoryY0> builder)
    {
        builder.ConfigureBaseEntity();

        builder.HasIndex(h => h.SkuSubId);

        builder.HasData(
            CreateHistoryY0(new Guid("86B1D24B-1E50-4355-81A1-EF0FD9DD912B"), Guid.Parse("FA5F1C30-462B-4ECF-AFAD-CA15F56D4782"), 0m, 0m),
            CreateHistoryY0(new Guid("046A1942-2BEB-4724-AA83-215E83625015"), Guid.Parse("0C6CA973-A664-44B3-87EE-10ED8F4BEF19"), 0m, 0m),
            CreateHistoryY0(new Guid("0261470A-6F6D-45DF-8AFF-52920B25AFFB"), Guid.Parse("870A6EF2-52E7-4BC3-9B1D-B62063AE824A"), 0m, 0m)
        );
    }

    private static HistoryY0 CreateHistoryY0(Guid id, Guid SkuSubId, decimal amount, decimal units)
    {
        return new HistoryY0
        {
            Id = id,
            SkuSubId = SkuSubId,
            Amount = amount,
            Units = units
        };
    }
}
