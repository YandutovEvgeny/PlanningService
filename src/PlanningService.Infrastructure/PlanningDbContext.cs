using Microsoft.EntityFrameworkCore;
using PlanningService.Domain.Entities;
using PlanningService.Infrastructure.Configurations;

namespace PlanningService.Infrastructure;

public class PlanningDbContext : DbContext
{
    public DbSet<Sku> Skus { get; set; }
    public DbSet<SkuSub> SkuSubs { get; set; }
    public DbSet<HistoryY0> HistoryY0Members { get; set; }
    public DbSet<PlanningY1> PlanningY1Members { get; set; }

    public PlanningDbContext()
    {
    }

    public PlanningDbContext(DbContextOptions<PlanningDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new SkuConfiguration());
        modelBuilder.ApplyConfiguration(new SkuSubConfiguration());
        modelBuilder.ApplyConfiguration(new HistoryY0Configuration());
        modelBuilder.ApplyConfiguration(new PlanningY1Configuration());
    }
}
