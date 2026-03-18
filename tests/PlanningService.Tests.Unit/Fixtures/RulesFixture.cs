using PlanningService.Application.Interfaces;
using System.Reflection;

namespace PlanningService.Tests.Unit.Fixtures;

public class RulesFixture
{
    public IReadOnlyDictionary<Type, IFormulaRule> Rules { get; }

    public RulesFixture()
    {
        var assembly = Assembly.GetAssembly(typeof(IFormulaRule));

        var ruleTypes = assembly!.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(IFormulaRule).IsAssignableFrom(t))
            .ToList();

        var rulesDictionary = new Dictionary<Type, IFormulaRule>();

        foreach (var type in ruleTypes)
        {
            if (Activator.CreateInstance(type) is IFormulaRule rule)
            {
                rulesDictionary[type] = rule;
                continue;
            }

            throw new InvalidOperationException($"Can not create formula rule of type: {type.Name}");
        }

        Rules = rulesDictionary;
    }
}