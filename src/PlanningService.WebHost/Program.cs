using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PlanningService.Infrastructure;
using PlanningService.Infrastructure.Extensions;
using PlanningService.WebHost.Exceptions;
using PlanningService.WebHost.Extensions;
using PlanningService.WebHost.Extensions.Configuration;
using PlanningService.WebHost.Extensions.ServiceCollection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseKestrel((context, option) => { option.Configure(context.Configuration.GetSection("Kestrel")); })
    .ConfigureAppConfiguration((context, configBuilder) =>
    {
        var env = context.HostingEnvironment.EnvironmentName;

        configBuilder.AddJsonFile("appsettings.json");
        configBuilder.AddJsonFile($"appsettings.{env}.json", optional: true);
        configBuilder.AddJsonFile($"Configs/ConnectionStrings.{env}.json", optional: true);
        configBuilder.AddJsonFile($"Configs/DatabaseNames.{env}.json", optional: true);
        configBuilder.AddEnvironmentVariables();
    });


builder.Logging.AddConsole();
builder.Services.AddControllers()
    .AddJsonOptions(opt => opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())); ;

var connection = new SqliteConnection(builder.Configuration.GetConnectionString("PlanningDbContext"));
connection.Open();

builder.Services.AddPlanningServiceDbContext(options =>
{
    options.DatabaseName = builder.Configuration.GetDatabaseName("PlanningServiceDbContext");
    options.Connection = connection;
});

builder.Services.AddSwaggerConfiguration();
builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddSingleton(connection);
builder.Services.AddServices();

var app = builder.Build();

app.UseExceptionHandler();
app.UseSwaggerConfiguration();
app.UseRouting();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PlannerDbContext>();

    context.Database.EnsureCreated();
    context.Database.Migrate();
}

app.Run();