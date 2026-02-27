using Exhibition.Core.Drivers;
using Exhibition.Core.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.Modules;

namespace Exhibition.Core;

public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        // Register content parts
        services.AddContentPart<HeroBannerPart>()
            .UseDisplayDriver<HeroBannerPartDisplayDriver>();

        services.AddContentPart<ExhibitorCardPart>()
            .UseDisplayDriver<ExhibitorCardPartDisplayDriver>();

        services.AddContentPart<ProductGalleryPart>()
            .UseDisplayDriver<ProductGalleryPartDisplayDriver>();

        services.AddContentPart<SchedulePart>()
            .UseDisplayDriver<SchedulePartDisplayDriver>();

        services.AddContentPart<ContactFormPart>()
            .UseDisplayDriver<ContactFormPartDisplayDriver>();
    }

    public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
    {
        // No additional route configuration needed for widgets
    }
}
