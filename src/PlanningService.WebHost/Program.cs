using PlanningService.Infrastructure.Extensions;
using PlanningService.WebHost.Exceptions;
using PlanningService.WebHost.Extensions;
using PlanningService.WebHost.Extensions.Configuration;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseKestrel((context, option) => { option.Configure(context.Configuration.GetSection("Kestrel")); })
    .ConfigureAppConfiguration((context, configBuilder) =>
    {
        var env = context.HostingEnvironment.EnvironmentName;

        configBuilder.AddJsonFile("appsettings.json");
        configBuilder.AddJsonFile($"appsettings.{env}.json", optional: true);
        configBuilder.AddEnvironmentVariables();
    });

builder.Services.AddPlanningServiceDbContext(options =>
{
    options.DatabaseName = builder.Configuration.GetDatabaseName("PlanningServiceDbContext");
});

builder.Logging.AddConsole();
builder.Services.AddControllers()
    .AddJsonOptions(opt => opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));;

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ExceptionHandler>();

builder.Services.AddCustomServices();

var app = builder.Build();

app.MapGet("/", () =>
{

    return "Hello World!";
});

app.Run();
