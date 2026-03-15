using Microsoft.EntityFrameworkCore;
using PlanningService.Domain.Entities;
using PlanningService.Domain.Interfaces;
using PlanningService.Infrastructure.Exceptions;

namespace PlanningService.Infrastructure.Repositories;

public class PlannerRepository : IPlannerRepository
{
    private readonly PlannerDbContext _context;

    public PlannerRepository(PlannerDbContext context)
    {
        _context = context;
    }

    public async Task<List<Sku>> GetAllSkusAsync(CancellationToken cancellationToken)
    {
        return await _context.Skus
            .Include(s => s.SubItems)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<List<SkuSub>> GetAllSkuSubsAsync(CancellationToken cancellationToken)
    {
        return await _context.SkuSubs
            .Include(s => s.Sku)
            .Include(s => s.HistoryMember)
            .Include(s => s.PlanningMember)
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

    public async Task<Guid> UpdatePlanningAsync(Guid skuSubId, decimal units, CancellationToken cancellationToken)
    {
        var skuSub = await _context.SkuSubs
            .Where(ss => ss.Id == skuSubId)
            .Include(ss => ss.PlanningMember)
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException($"Sku sub with id {skuSubId} not found");

        if (skuSub.PlanningMember is not null)
        {
            skuSub.PlanningMember.Units = units;
            skuSub.PlanningMember.Amount = units * skuSub.Price;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return skuSub.Id;
    }
}