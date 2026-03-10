using PlanningService.Application.Models;

namespace PlanningService.Application.Interfaces;

public interface ICalculationEngine
{
    void Calculate(ICalculationContext context);
}