using PlanningService.Application.Interfaces;
using PlanningService.Application.Models;
using PlanningService.Application.Rules.Sku.Price;
using PlanningService.Tests.Unit.Fixtures;

namespace PlanningService.Tests.Unit.Rules.Sku;

public class SkuPriceRulesTests :
    IClassFixture<CalculationContextFixture>,
    IClassFixture<RulesFixture>
{
    private readonly CalculationContext _context;
    private readonly IReadOnlyDictionary<Type, IFormulaRule> _rules;

    public SkuPriceRulesTests(CalculationContextFixture contextFixture, RulesFixture rulesFixture)
    {
        _context = contextFixture.Context;
        _rules = rulesFixture.Rules;
    }

    [Fact]
    public void Apply_ValidAmountAndUnits_ShouldSetCorrectPlanningPrice()
    {
        //Arrange
        var amount = 10m;
        var units = 5m;
        var expectedValue = 2m;

        var skuNode = new SkuNode
        {
            AmountPlanning = amount,
            UnitsPlanning = units
        };

        var rule = _rules[typeof(SkuPricePlanningY1Rule)];

        //Act
        rule.Apply(skuNode, _context);

        //Assert
        Assert.Equal(expectedValue, skuNode.PricePlanning);
    }

    [Fact]
    public void Apply_ValidAmountAndUnits_ShouldSetCorrectHistoryPrice()
    {
        //Arrange
        var amount = 100m;
        var units = 5m;
        var expectedValue = 20m;

        var skuNode = new SkuNode
        {
            AmountHistory = amount,
            UnitsHistory = units
        };

        var rule = _rules[typeof(SkuPriceHistoryY0Rule)];

        //Act
        rule.Apply(skuNode, _context);

        //Assert
        Assert.Equal(expectedValue, skuNode.PriceHistory);
    }

    [Fact]
    public void Apply_ValidSkuHistoryAndPlanningPriceAndTotalHistoryPrice_ShouldSetCorrectGrowthPrice()
    {
        //Arrange
        var pricePlanning = 100m;
        var priceHistory = 50m;
        var totalPriceHistory = 10m;
        var expectedValue = 5m;

        var skuNode = new SkuNode
        {
            PricePlanning = pricePlanning,
            PriceHistory = priceHistory
        };

        _context.Total!.PriceHistory = totalPriceHistory;
        var rule = _rules[typeof(SkuPriceGrowthRule)];

        //Act
        rule.Apply(skuNode, _context);

        //Assert
        Assert.Equal(expectedValue, skuNode.PriceGrowth);
    }
}