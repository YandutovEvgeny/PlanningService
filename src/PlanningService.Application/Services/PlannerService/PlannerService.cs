using Microsoft.Extensions.Logging;
using PlanningService.Application.Contracts.Planner;
using PlanningService.Application.Interfaces;
using PlanningService.Application.Models;
using PlanningService.Domain.Entities;
using PlanningService.Domain.Interfaces;
using ValueType = PlanningService.Application.Contracts.Planner.Enums.ValueType;
using Alignment = PlanningService.Application.Contracts.Planner.Enums.Alignment;
using PlanningService.Application.Contracts;
using PlanningService.Application.Contracts.Planner.Enums;

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

        var skuSubNodes = new List<SkuSubNode>();
        var skuNodes = new List<SkuNode>();
        TotalNode? totalNode = null;

        skuSubNodes = BuildSkuSubNodes(filteredSkuSubs, historyDictionary, planningDictionary);

        if (filter.Level is Level.Sku)
        {
            skuNodes = BuildSkuNodes(skus, skuSubNodes);
        }

        if (filter.Level is Level.Total)
        {
            skuNodes = BuildSkuNodes(skus, skuSubNodes);
            totalNode = new TotalNode();
        }

        var context = new CalculationContext
        {
            Total = totalNode,
            Skus = skuNodes,
            SkuSubs = skuSubNodes
        };

        _engine.Calculate(context);

        (List<PlannerRow> rows, List<MetadataModel> metadata) = BuildPlannerRowsAndMetadata(filter.Level, totalNode, skuNodes, skuSubNodes);

        return new PlannerResponse
        {
            Data = rows,
            Metadata = metadata
        };
    }

    public async Task<ResponseId<Guid>> UpdatePlanningAsync(Guid skuSubId, UpdatePlanningDto data, CancellationToken cancellationToken = default)
    {
        var result = await _plannerRepository.UpdatePlanningAsync(skuSubId, data.Units, cancellationToken);

        return new ResponseId<Guid>
        {
            Id = result
        };
    }

    private (List<PlannerRow>, List<MetadataModel>) BuildPlannerRowsAndMetadata(
        Level level,
        TotalNode? totalNode,
        List<SkuNode> skuNodes,
        List<SkuSubNode> skuSubNodes)
    {
        var rows = new List<PlannerRow>();
        var metadata = new List<MetadataModel>();

        foreach (var skuSub in skuSubNodes)
        {
            FillSkuSubRowsAndMetadata(rows, metadata, skuSub);
        }

        if (level is Level.Sku)
        {
            foreach (var sku in skuNodes)
            {
                FillSkuRowsAndMetadata(rows, metadata, sku);
            }
        }

        if (level is Level.Total && totalNode is not null)
        {
            foreach (var sku in skuNodes)
            {
                FillSkuRowsAndMetadata(rows, metadata, sku);
            }

            FillTotalRowAndMetadata(rows, metadata, totalNode);
        }

        _logger.LogInformation("Planner rows and metadata with level:{Level} builded successfully", level);

        return (rows, metadata);
    }

    private void FillSkuSubRowsAndMetadata(List<PlannerRow> rows, List<MetadataModel> metadata, SkuSubNode node)
    {
        var rowValueInfoDict = GetRowValueInfo(node)
            .GroupBy(i => i.Type)
            .ToDictionary(i => i.Key, i => i.ToList());

        rows.Add(new PlannerRow
        {
            Level = Level.SkuSub,
            Title = node.Name,
            ParentId = node.ParentNode?.Id ?? Guid.Empty,
            UnitsInfos = rowValueInfoDict[ValueType.UNITS].Select(v =>
            {
                var id = Guid.NewGuid();

                metadata.Add(new MetadataModel
                {
                    Id = id,
                    DataType = "number",
                    Title = v.Name,
                    Style = Alignment.Center,
                    IsEditable = v.Column is Column.PlanningY1
                });

                return new UnitsInfo
                {
                    MetadataId = id,
                    Column = v.Column,
                    Value = v.Value
                };
            }).ToList(),
            PriceInfos = rowValueInfoDict[ValueType.PRICE].Select(v =>
            {
                var id = Guid.NewGuid();

                metadata.Add(new MetadataModel
                {
                    Id = id,
                    DataType = "number",
                    Title = v.Name,
                    Style = Alignment.Center,
                    IsEditable = v.Column is Column.PlanningY1
                });

                return new PriceInfo
                {
                    MetadataId = id,
                    Column = v.Column,
                    Value = v.Value
                };
            }).ToList(),
            AmountInfos = rowValueInfoDict[ValueType.AMOUNT].Select(v =>
            {
                var id = Guid.NewGuid();

                metadata.Add(new MetadataModel
                {
                    Id = id,
                    DataType = "number",
                    Title = v.Name,
                    Style = Alignment.Center,
                    IsEditable = v.Column is Column.PlanningY1
                });

                return new AmountInfo
                {
                    MetadataId = id,
                    Column = v.Column,
                    Value = v.Value
                };
            }).ToList()
        });
    }

    private void FillSkuRowsAndMetadata(List<PlannerRow> rows, List<MetadataModel> metadata, SkuNode node)
    {
        var rowValueInfoDict = GetRowValueInfo(node)
            .GroupBy(v => v.Type)
            .ToDictionary(v => v.Key, v => v.ToList());

        rows.Add(new PlannerRow
        {
            Level = Level.Sku,
            Title = node.Name,
            ParentId = Guid.Empty,
            UnitsInfos = rowValueInfoDict[ValueType.UNITS].Select(v =>
            {
                var id = Guid.NewGuid();

                metadata.Add(new MetadataModel
                {
                    Id = id,
                    DataType = "number",
                    Title = v.Name,
                    Style = Alignment.Center,
                    IsEditable = false
                });

                return new UnitsInfo
                {
                    MetadataId = id,
                    Column = v.Column,
                    Value = v.Value
                };
            }).ToList(),
            PriceInfos = rowValueInfoDict[ValueType.PRICE].Select(v =>
            {
                var id = Guid.NewGuid();

                metadata.Add(new MetadataModel
                {
                    Id = id,
                    DataType = "number",
                    Title = v.Name,
                    Style = Alignment.Center,
                    IsEditable = false
                });

                return new PriceInfo
                {
                    MetadataId = id,
                    Column = v.Column,
                    Value = v.Value
                };
            }).ToList(),
            AmountInfos = rowValueInfoDict[ValueType.AMOUNT].Select(v =>
            {
                var id = Guid.NewGuid();

                metadata.Add(new MetadataModel
                {
                    Id = id,
                    DataType = "number",
                    Title = v.Name,
                    Style = Alignment.Center,
                    IsEditable = v.Column is Column.PlanningY1
                });

                return new AmountInfo
                {
                    MetadataId = id,
                    Column = v.Column,
                    Value = v.Value
                };
            }).ToList()
        });
    }

    private void FillTotalRowAndMetadata(List<PlannerRow> rows, List<MetadataModel> metadata, TotalNode node)
    {
        var rowValueInfo = GetRowValueInfo(node)
            .GroupBy(v => v.Type)
            .ToDictionary(v => v.Key, v => v.ToList());

        rows.Add(new PlannerRow
        {
            Level = Level.Total,
            Title = "Total",
            ParentId = Guid.Empty,
            UnitsInfos = rowValueInfo[ValueType.UNITS].Select(v =>
            {
                var id = Guid.NewGuid();

                metadata.Add(new MetadataModel
                {
                    Id = id,
                    DataType = "number",
                    Title = v.Name,
                    Style = Alignment.Center,
                    IsEditable = false,
                });

                return new UnitsInfo
                {
                    MetadataId = id,
                    Column = v.Column,
                    Value = v.Value
                };
            }).ToList(),
            PriceInfos = rowValueInfo[ValueType.PRICE].Select(v =>
            {
                var id = Guid.NewGuid();

                metadata.Add(new MetadataModel
                {
                    Id = id,
                    DataType = "number",
                    Title = v.Name,
                    Style = Alignment.Center,
                    IsEditable = false
                });

                return new PriceInfo
                {
                    MetadataId = id,
                    Column = v.Column,
                    Value = v.Value
                };
            }).ToList(),
            AmountInfos = rowValueInfo[ValueType.AMOUNT].Select(v =>
            {
                var id = Guid.NewGuid();

                metadata.Add(new MetadataModel
                {
                    Id = id,
                    DataType = "number",
                    Title = v.Name,
                    Style = Alignment.Center,
                    IsEditable = false
                });

                return new AmountInfo
                {
                    MetadataId = id,
                    Column = v.Column,
                    Value = v.Value
                };
            }).ToList()
        });
    }

    private List<SkuSubNode> BuildSkuSubNodes(
        IEnumerable<SkuSub> skuSubs,
        Dictionary<Guid, HistoryY0> historyDictionary,
        Dictionary<Guid, PlanningY1> planningDictionary)
    {
        var result = new List<SkuSubNode>();

        foreach (var skuSub in skuSubs)
        {
            var history = historyDictionary.GetValueOrDefault(skuSub.Id) ?? new HistoryY0 { Amount = 0m, Units = 0m };
            var planning = planningDictionary.GetValueOrDefault(skuSub.Id) ?? new PlanningY1 { Amount = 0m, Units = 0m };

            result.Add(new SkuSubNode
            {
                ParentNode = new SkuNode
                {
                    Id = skuSub.SkuId,
                    Name = skuSub.Sku.Name
                },
                Id = skuSub.Id,
                Name = skuSub.Name,
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
            .GroupBy(ss => ss.ParentNode.Id)
            .ToDictionary(g => g.Key, g => g.ToList());

        foreach (var sku in skus)
        {
            var childs = skuSubDictionary.GetValueOrDefault(sku.Id);

            if (childs is null) continue;

            var skuNode = new SkuNode
            {
                Id = sku.Id,
                Name = sku.Name,
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

        foreach (var (name, getter) in propertyGetters)
        {
            result.Add(new ValueInfo
            {
                Name = name,
                Type = name.StartsWith("Price")
                    ? ValueType.PRICE
                    : name.StartsWith("Amount")
                        ? ValueType.AMOUNT
                        : ValueType.UNITS,
                Column = name.EndsWith("Planning")
                    ? Column.PlanningY1
                    : name.EndsWith("Growth")
                        ? Column.ContributionGrowth
                        : Column.HistoryY0,
                Value = getter(node)
            });
        }

        return result;
    }
}