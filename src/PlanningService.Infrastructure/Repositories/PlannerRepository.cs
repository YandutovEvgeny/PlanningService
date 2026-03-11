using Microsoft.EntityFrameworkCore;
using PlanningService.Domain.Entities;
using PlanningService.Domain.Interfaces;

namespace PlanningService.Infrastructure.Repositories;

public class PlannerRepository : IPlannerRepository
{
    private readonly PlanningDbContext _context;

    public PlannerRepository(PlanningDbContext context)
    {
        _context = context;
    }

    public async Task<List<Sku>> GetAllSkusAsync(CancellationToken cancellationToken)
    {
        return await _context.Skus
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<List<SkuSub>> GetAllSkuSubsAsync(CancellationToken cancellationToken)
    {
        return await _context.SkuSubs
            .Include(s => s.Sku)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Dictionary<Guid, HistoryY0>> GetHistoryBySkuSubIdsAsync(IEnumerable<Guid> skuSubIds, CancellationToken cancellationToken)
    {
        return await _context.HistoryY0Members
            .Where(h => skuSubIds.Contains(h.SkuSubId))
            .AsNoTracking()
            .ToDictionaryAsync(h => h.SkuSubId, cancellationToken);
    }

    public async Task<Dictionary<Guid, PlanningY1>> GetPlanningBySkuSubIdsAsync(IEnumerable<Guid> skuSubIds, CancellationToken cancellationToken)
    {
        return await _context.PlanningY1Members
            .Where(p => skuSubIds.Contains(p.SkuSubId))
            .AsNoTracking()
            .ToDictionaryAsync(p => p.SkuSubId, cancellationToken);
    }

    public async Task UpdatePlanningAsync(Guid skuSubId, decimal newValue, CancellationToken cancellationToken)
    {
        var planning = await _context.PlanningY1Members.FindAsync([skuSubId], cancellationToken: cancellationToken);
        var skuSub = await _context.SkuSubs.FindAsync([skuSubId], cancellationToken: cancellationToken)
            ?? throw new ArgumentException("SkuSub not found");

        if (planning is null)
        {
            planning = new PlanningY1
            {
                SkuSubId = skuSubId
            };
            _context.PlanningY1Members.Add(planning);
        }

        planning.Units = newValue;
        planning.Amount = newValue * skuSub.Price;

        await _context.SaveChangesAsync(cancellationToken);
    }
}