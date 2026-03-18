using PlanningService.Application.Interfaces;
using PlanningService.Application.Models;
using PlanningService.Application.Rules.Sku.Units;
using PlanningService.Tests.Unit.Fixtures;

namespace PlanningService.Tests.Unit.Rules.Sku;

public class SkuUnitsRulesTests :
    IClassFixture<CalculationContextFixture>,
    IClassFixture<RulesFixture>
{
    private readonly CalculationContext _context;
    private readonly IReadOnlyDictionary<Type, IFormulaRule> _rules;

    public SkuUnitsRulesTests(CalculationContextFixture contextFixture, RulesFixture rulesFixture)
    {
        _context = contextFixture.Context;
        _rules = rulesFixture.Rules;
    }

    [Fact]
    public void Apply_ValidSkuSubUnitsAndRatio_ShouldSetCorrectPlanningUnits()
    {
        //Arrange
        var units = 10m;
        var ratio = 5m;
        var expectedValue = 250m;

        var skuNode = new SkuNode
        {
            Childrens = Enumerable.Range(1, 5).Select(c => new SkuSubNode
            {
                ParentNode = new SkuNode(),
                UnitsPlanning = units,
                Ratio = ratio
            }).ToList()
        };

        var rule = _rules[typeof(SkuUnitsPlanningY1Rule)];

        //Act
        rule.Apply(skuNode, _context);

        //Assert
        Assert.Equal(expectedValue, skuNode.UnitsPlanning);
    }

    [Fact]
    public void Apply_ValidSkuSubUnitsAndRatio_ShouldSetCorrectHistoryUnits()
    {
        //Arrange
        var units = 5m;
        var ratio = 5m;
        var expectedValue = 125m;

        var skuNode = new SkuNode
        {
            Childrens = Enumerable.Range(1, 5).Select(c => new SkuSubNode
            {
                ParentNode = new SkuNode(),
                UnitsHistory = units,
                Ratio = ratio
            }).ToList()
        };

        var rule = _rules[typeof(SkuUnitsHistoryY0Rule)];

        //Act
        rule.Apply(skuNode, _context);

        //Assert
        Assert.Equal(expectedValue, skuNode.UnitsHistory);
    }

    [Fact]
    public void Apply_ValidSkuHistoryAndPlanningUnitsAndTotalHistoryUnits_ShouldSetCorrectGrowthUnits()
    {
        //Arrange
        var unitsPlanning = 100m;
        var unitsHistory = 50m;
        var totalUnitsHistory = 10m;
        var expectedValue = 5m;

        var skuNode = new SkuNode
        {
            UnitsPlanning = unitsPlanning,
            UnitsHistory = unitsHistory
        };

        _context.Total!.UnitsHistory = totalUnitsHistory;
        var rule = _rules[typeof(SkuUnitsGrowthRule)];

        //Act
        rule.Apply(skuNode, _context);

        //Assert
        Assert.Equal(expectedValue, skuNode.UnitsGrowth);
    }
}