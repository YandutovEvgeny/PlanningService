using Microsoft.Extensions.Logging;
using PlanningService.Application.Contracts.Planner;
using PlanningService.Application.Interfaces;
using PlanningService.Application.Models;
using PlanningService.Domain.Entities;
using PlanningService.Domain.Interfaces;
using ValueType = PlanningService.Application.Contracts.Planner.Enums.ValueType;
using Alignment = PlanningService.Application.Contracts.Planner.Enums.Alignment;
using PlanningService.Application.Contracts;

namespace PlanningService.Application.Services.PlannerService;

public class PlannerService : IPlannerService
{
    private readonly IPlannerRepository _plannerRepository;
    private readonly ICalculationEngine _engine;
    private readonly ILogger<PlannerService> _logger;

    public PlannerService(
        IPlannerRepository plannerRepository,
        ICalculationEngine engine,
        ILogger<PlannerService> logger)
    { 
        _plannerRepository = plannerRepository;
        _engine = engine;
        _logger = logger;
    }

    public async Task<PlannerResponse> GetPlannerDataAsync(PlannerFilter filter, CancellationToken cancellationToken = default)
    {
        var skus = await _plannerRepository.GetAllSkusAsync(cancellationToken);
        var skuSubs = await _plannerRepository.GetAllSkuSubsAsync(cancellationToken);
        var historyDictionary = await _plannerRepository.GetHistoryBySkuSubIdsAsync(skuSubs.Select(ss => ss.Id), cancellationToken);
        var planningDictionary = await _plannerRepository.GetPlanningBySkuSubIdsAsync(skuSubs.Select(ss => ss.Id), cancellationToken);
        var filteredSkuSubs = skuSubs;

        if (filter.SkuSubNames is not null && filter.SkuSubNames.Length != 0)
        {
            filteredSkuSubs = [.. skuSubs.Where(ss => filter.SkuSubNames.Contains(ss.Name))];
        }

        var skuSubNodes = BuildSkuSubNodes(filteredSkuSubs, historyDictionary, planningDictionary);
        var skuNodes = BuildSkuNodes(skus, skuSubNodes);
        var totalNode = new TotalNode();

        var context = new CalculationContext
        {
            Total = totalNode,
            Skus = skuNodes
        };

        _engine.Calculate(context);

        var rows = BuildPlannerRow(filter.Levels, totalNode, skuNodes, skuSubNodes);

        var metadata = BuildColumnMetadata(rows);

        return new PlannerResponse
        {
            Data = rows,
            Metadata = metadata
        };
    }

    public async Task<ResponseId<Guid>> UpdatePlanningAsync(Guid skuSubId, decimal units, CancellationToken cancellationToken = default)
    {
        var result = await _plannerRepository.UpdatePlanningAsync(skuSubId, units, cancellationToken);

        return new ResponseId<Guid>
        {
            Id = result
        };
    }

    private List<ColumnMetadata> BuildColumnMetadata(IEnumerable<PlannerRow> rows)
    {
        if (rows is null) return [];

        var metadata = new List<ColumnMetadata>();

        foreach (var row in rows)
        {
            metadata.Add(new ColumnMetadata
            {
                Level = row.Level,
                DataType = "number",
                Title = row.Title,
                Style = Alignment.Center,
                IsEditable = row.Level is "SkuSub" && row.ValueInfo.Name is "PlanningY1"
            });
        }

        return metadata;
    }

    private List<PlannerRow> BuildPlannerRow(
        string[]? levels,
        TotalNode totalNode,
        List<SkuNode> skuNodes,
        List<SkuSubNode> skuSubNodes)
    {
        var rows = new List<PlannerRow>();

        levels = levels?.Select(l => l.ToLower()).ToArray();

        if (levels is null || levels.Contains("total"))
        {
            AddTotalRows(rows, totalNode);
        }

        if (levels is null || levels.Contains("sku"))
        {
            foreach (var sku in skuNodes.OrderBy(sku => sku.SkuName))
            {
                AddSkuRows(rows, sku);
            }
        }

        if (levels is null || levels.Contains("skusub"))
        {
            foreach(var skuSub in skuSubNodes.OrderBy(ss => ss.ParentNode?.SkuName).ThenBy(ss => ss.SkuSubName))
            {
                AddSkuSubRows(rows, skuSub);
            }
        }

        return rows;
    }

    private void AddSkuSubRows(List<PlannerRow> rows, SkuSubNode node)
    {
        var rowValueInfo = GetRowValueInfo(node);

        foreach(var valueInfo in rowValueInfo)
        {
            rows.Add(new PlannerRow
            {
                Level = "SkuSub",
                Title = node.SkuSubName,
                ValueInfo = valueInfo
            });
        }
    }

    private void AddSkuRows(List<PlannerRow> rows, SkuNode node)
    {
        var rowValueInfo = GetRowValueInfo(node);

        foreach(var valueInfo in rowValueInfo)
        {
            rows.Add(new PlannerRow
            {
                Level = "Sku",
                Title = node.SkuName,
                ValueInfo = valueInfo
            });
        }
    }

    private void AddTotalRows(List<PlannerRow> rows, TotalNode node)
    {
        var rowValueInfo = GetRowValueInfo(node);

        foreach(var valueInfo in rowValueInfo)
        {
            rows.Add(new PlannerRow
            {
                Level = "Total",
                Title = "Total",
                ValueInfo = valueInfo
            });
        }
    }

    private List<SkuSubNode> BuildSkuSubNodes(
        IEnumerable<SkuSub> skuSubs,
        Dictionary<Guid, HistoryY0> historyDictionary,
        Dictionary<Guid, PlanningY1> planningDictionary)
    {
        var result = new List<SkuSubNode>();

        foreach(var skuSub in skuSubs)
        {
            var history = historyDictionary.GetValueOrDefault(skuSub.Id) ?? new HistoryY0 { Amount = 0m, Units = 0m };
            var planning = planningDictionary.GetValueOrDefault(skuSub.Id) ?? new PlanningY1 { Amount = 0m, Units = 0m };

            result.Add(new SkuSubNode
            {
                ParentNode = new SkuNode
                {
                    SkuId = skuSub.SkuId,
                    SkuName = skuSub.Sku.Name
                },
                SkuSubId = skuSub.Id,
                SkuSubName = skuSub.Name,
                Ratio = skuSub.Ratio,
                UnitsHistory = history.Units,
                UnitsPlanning = planning.Units,
                AmountHistory = history.Amount,
                AmountPlanning = planning.Amount,
                Price = skuSub.Price
            });
        }

        return result;
    }

    private List<SkuNode> BuildSkuNodes(IEnumerable<Sku> skus, List<SkuSubNode> skuSubNodes)
    {
        var skuNodes = new List<SkuNode>();
        var skuSubDictionary = skuSubNodes
            .GroupBy(ss => ss.ParentNode.SkuId)
            .ToDictionary(g => g.Key, g => g.ToList());

        foreach (var sku in skus)
        {
            var childs = skuSubDictionary.GetValueOrDefault(sku.Id);

            if (childs is null) continue;

            var skuNode = new SkuNode
            {
                SkuId = sku.Id,
                SkuName = sku.Name,
                Childrens = childs
            };

            foreach (var child in childs)
            {
                child.ParentNode = skuNode;
            }

            skuNodes.Add(skuNode);
        }

        return skuNodes;
    }

    private List<ValueInfo> GetRowValueInfo(CalculationNodeBase node)
    {
        var propertyGetters = new List<(string name, Func<CalculationNodeBase, decimal> getter)>(9)
        {
            (nameof(CalculationNodeBase.UnitsHistory), d => d.UnitsHistory),
            (nameof(CalculationNodeBase.UnitsPlanning), d => d.UnitsPlanning),
            (nameof(CalculationNodeBase.UnitsGrowth), d => d.UnitsGrowth),
            (nameof(CalculationNodeBase.PriceHistory), d => d.PriceHistory),
            (nameof(CalculationNodeBase.PricePlanning), d => d.PricePlanning),
            (nameof(CalculationNodeBase.PriceGrowth), d => d.PriceGrowth),
            (nameof(CalculationNodeBase.AmountHistory), d => d.AmountHistory),
            (nameof(CalculationNodeBase.AmountPlanning), d => d.AmountPlanning),
            (nameof(CalculationNodeBase.AmountGrowth), d => d.AmountGrowth)
        };

        var result = new List<ValueInfo>();

        foreach(var (name, getter) in propertyGetters)
        {
            var valueType = ValueType.UNITS;

            if (name.StartsWith("Price"))
            {
                valueType = ValueType.PRICE;
            }

            if (name.StartsWith("Amount"))
            {
                valueType = ValueType.AMOUNT;
            }

            result.Add(new ValueInfo
            {
                Name = name,
                Type = valueType,
                Value = getter(node)
            });
        }

        return result;
    }
}