# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files first for layer caching
COPY OrchardCore.Practice.sln ./
COPY src/OrchardCore.Practice.Web/OrchardCore.Practice.Web.csproj ./src/OrchardCore.Practice.Web/
COPY src/OrchardCore.Practice.Modules/Exhibition.Core/Exhibition.Core.csproj ./src/OrchardCore.Practice.Modules/Exhibition.Core/
COPY src/OrchardCore.Practice.Modules/Exhibition.AI/Exhibition.AI.csproj ./src/OrchardCore.Practice.Modules/Exhibition.AI/
COPY src/OrchardCore.Practice.Modules/Exhibition.MultiTenant/Exhibition.MultiTenant.csproj ./src/OrchardCore.Practice.Modules/Exhibition.MultiTenant/
COPY src/OrchardCore.Practice.Themes/ExhibitionTheme/ExhibitionTheme.csproj ./src/OrchardCore.Practice.Themes/ExhibitionTheme/
COPY tests/Exhibition.Tests/Exhibition.Tests.csproj ./tests/Exhibition.Tests/

# Restore NuGet packages
RUN dotnet restore

# Copy all source files
COPY . .

# Build and publish
RUN dotnet publish src/OrchardCore.Practice.Web/OrchardCore.Practice.Web.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Create non-root user for security
RUN groupadd -r appuser && useradd -r -g appuser appuser

# Copy published files
COPY --from=build /app/publish .

# Create directories for Orchard Core data
RUN mkdir -p /app/App_Data /app/wwwroot/media && \
    chown -R appuser:appuser /app

USER appuser

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Production

HEALTHCHECK --interval=30s --timeout=10s --start-period=60s --retries=3 \
    CMD curl -f http://localhost:8080/healthz || exit 1

ENTRYPOINT ["dotnet", "OrchardCore.Practice.Web.dll"]
