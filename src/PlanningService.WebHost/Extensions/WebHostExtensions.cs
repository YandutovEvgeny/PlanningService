using PlanningService.WebHost.Consts;
using Microsoft.OpenApi;

namespace PlanningService.WebHost.Extensions;

public static class WebHostExtensions
{
    private const string AppVersion = "V1";
    private const string ControllerSectionUrl = "/swagger/{0}/swagger.json";
    private const string ControllerSectionName = $"{{0}} {AppVersion}";

    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(ControllerSections.PlannerApi,
                new OpenApiInfo { Title = ControllerSections.PlannerApiName, Version = AppVersion });

            var xmlDocs = Directory.GetFiles(AppContext.BaseDirectory, "*.xml").ToList();
            
            xmlDocs.ForEach(xmlDoc => options.IncludeXmlComments(xmlDoc));
            options.UseInlineDefinitionsForEnums();
        });

        return services;
    }

    public static WebApplication UseSwaggerConfiguration(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(opt =>
        {
            opt.SwaggerEndpoint(
                string.Format(ControllerSectionUrl, ControllerSections.PlannerApi),
                string.Format(ControllerSectionName, ControllerSections.PlannerApiName));
        });
        
        return app;
    }
}