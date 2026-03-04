using Microsoft.Extensions.Options;

namespace PlanningService.Infrastructure.Options;

public class PlanningServiceDbContextOptions : IValidateOptions<PlanningServiceDbContextOptions>
{
    public string? DatabaseName { get; set; }

    public ValidateOptionsResult Validate(string? name, PlanningServiceDbContextOptions options)
    {
        return string.IsNullOrEmpty(name)
            ? ValidateOptionsResult.Fail("Database name is required")
            : ValidateOptionsResult.Success;
    }
}
