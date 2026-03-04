namespace PlanningService.WebHost.Extensions.Configuration;

public static class ConfigurationExtensions
{
    public static string? GetDatabaseName(this IConfiguration configuration, string name)
    {
        return configuration.GetSection("DatabaseNames")[name];
    }
}
