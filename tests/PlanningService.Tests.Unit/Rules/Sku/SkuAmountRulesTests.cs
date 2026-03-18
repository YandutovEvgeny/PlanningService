using PlanningService.Application.Interfaces;
using PlanningService.Application.Models;
using PlanningService.Application.Rules.Sku.Amount;
using PlanningService.Tests.Unit.Fixtures;

namespace PlanningService.Tests.Unit.Rules.Sku;

public class SkuAmountRulesTests :
    IClassFixture<CalculationContextFixture>,
    IClassFixture<RulesFixture>
{
    private readonly CalculationContext _context;
    private readonly IReadOnlyDictionary<Type, IFormulaRule> _rules;

    public SkuAmountRulesTests(CalculationContextFixture contextFixture, RulesFixture rulesFixture)
    {
        _context = contextFixture.Context;
        _rules = rulesFixture.Rules;
    }

    [Fact]
    public void Apply_ValidPlanningAmount_ShouldSetCorrectAmount()
    {
        //Arrange
        var amount = 100m;
        var expectedValue = 500m;

        var skuNode = new SkuNode
        {
            Childrens = Enumerable.Range(1, 5).Select(c => new SkuSubNode
            {
                ParentNode = new SkuNode(),
                AmountPlanning = amount
            }).ToList()
        };

        var rule = _rules[typeof(SkuAmountPlanningY1Rule)];

        //Act
        rule.Apply(skuNode, _context);

        //Assert
        Assert.Equal(expectedValue, skuNode.AmountPlanning);
    }

    [Fact]
    public void Apply_ValidHistoryAmount_ShouldSetCorrectAmount()
    {
        //Arrange
        var amount = 50m;
        var expectedValue = 250m;

        var skuNode = new SkuNode
        {
            Childrens = Enumerable.Range(1, 5).Select(c => new SkuSubNode
            {
                ParentNode = new SkuNode(),
                AmountHistory = amount
            }).ToList()
        };

        var rule = _rules[typeof(SkuAmountHistoryY0Rule)];

        //Act
        rule.Apply(skuNode, _context);

        //Assert
        Assert.Equal(expectedValue, skuNode.AmountHistory);
    }

    [Fact]
    public void Apply_ValidSkuHistoryAndPlanningAmountAndTotalHistoryAmount_ShouldSetCorrectGrowthAmount()
    {
        //Arrange
        var amountPlanning = 100m;
        var amountHistory = 50m;
        var totalAmountHistory = 10m;
        var expectedValue = 5m;

        var skuNode = new SkuNode
        {
            AmountPlanning = amountPlanning,
            AmountHistory = amountHistory
        };

        _context.Total!.AmountHistory = totalAmountHistory;
        var rule = _rules[typeof(SkuAmountGrowthRule)];

        //Act
        rule.Apply(skuNode, _context);

        //Assert
        Assert.Equal(expectedValue, skuNode.AmountGrowth);
    }
}