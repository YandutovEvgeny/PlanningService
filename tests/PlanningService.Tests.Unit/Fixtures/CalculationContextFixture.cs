using PlanningService.Application.Models;

namespace PlanningService.Tests.Unit.Fixtures;

public class CalculationContextFixture : IDisposable
{
    public CalculationContext Context { get; private set; }

    public CalculationContextFixture()
    {
        Context = new CalculationContext()
        {
            Total = new TotalNode()
        };
    }

    public void Dispose()
    {
        Context = null;
    }
}