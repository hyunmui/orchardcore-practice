using Exhibition.AI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace Exhibition.AI;

public class Startup(IConfiguration configuration) : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.Configure<AiOptions>(configuration.GetSection(AiOptions.SectionName));
        services.AddScoped<IPageGeneratorService, PageGeneratorService>();
    }

    public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
    {
        routes.MapAreaControllerRoute(
            name: "ExhibitionAI",
            areaName: "Exhibition.AI",
            pattern: "Admin/Exhibition/AI/{controller=PageGenerator}/{action=Index}/{id?}");
    }
}
