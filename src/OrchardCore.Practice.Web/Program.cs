var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOrchardCms()
    .AddTenantFeatures(
        "OrchardCore.Admin",
        "OrchardCore.Autoroute",
        "OrchardCore.ContentFields",
        "OrchardCore.ContentTypes",
        "OrchardCore.Contents",
        "OrchardCore.Flows",
        "OrchardCore.Liquid",
        "OrchardCore.Media",
        "OrchardCore.Settings",
        "OrchardCore.Tenants",
        "OrchardCore.Themes",
        "OrchardCore.Title"
    );

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();

app.UseOrchardCore();

app.Run();
