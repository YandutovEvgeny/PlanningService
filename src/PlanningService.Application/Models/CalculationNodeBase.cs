namespace PlanningService.Application.Models;

/// <summary>
/// Abstract base class of calculation node.
/// </summary>
public abstract class CalculationNodeBase : ICalculationNode
{
    /// <inheritdoc/>
    public abstract string Level { get;}

    /// <summary>
    /// Units history value.
    /// </summary>
    public decimal UnitsHistory { get; set; }

    /// <summary>
    /// Units planning value.
    /// </summary>
    public decimal UnitsPlanning { get; set; }

    /// <summary>
    /// Units growth value.
    /// </summary>
    public decimal UnitsGrowth { get; set; }

    /// <summary>
    /// Price's history value.
    /// </summary>
    public decimal PriceHistory { get; set; }

    /// <summary>
    /// Price's planning value.
    /// </summary>
    public decimal PricePlanning { get; set; }

    /// <summary>
    /// Price's growth value.
    /// </summary>
    public decimal PriceGrowth { get; set; }

    /// <summary>
    /// Amount's history value.
    /// </summary>
    public decimal AmountHistory { get; set; }

    /// <summary>
    /// Amounts's planning value.
    /// </summary>
    public decimal AmountPlanning { get; set; }

    /// <summary>
    /// Amount's growth value.
    /// </summary>
    public decimal AmountGrowth { get; set; }
}