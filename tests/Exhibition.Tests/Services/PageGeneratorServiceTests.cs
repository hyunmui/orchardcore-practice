using Exhibition.AI.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Exhibition.Tests.Services;

public class PageGeneratorServiceTests
{
    private readonly AiOptions _defaultOptions = new()
    {
        ModelId = "claude-sonnet-4-6",
        ApiKey = "test-api-key",
        Endpoint = "https://api.anthropic.com"
    };

    private PageGeneratorService CreateService(AiOptions? options = null)
    {
        var opts = Options.Create(options ?? _defaultOptions);
        var logger = NullLogger<PageGeneratorService>.Instance;
        return new PageGeneratorService(logger, opts);
    }

    [Fact]
    public void Service_CanBeInstantiated_WithValidOptions()
    {
        // Arrange & Act
        var service = CreateService();

        // Assert
        Assert.NotNull(service);
    }

    [Fact]
    public async Task GeneratePageAsync_WithEmptyApiKey_ReturnsFailureResult()
    {
        // Arrange
        var options = new AiOptions
        {
            ModelId = "claude-sonnet-4-6",
            ApiKey = string.Empty, // 빈 API 키
            Endpoint = "https://api.anthropic.com"
        };
        var service = CreateService(options);

        // Act
        var result = await service.GeneratePageAsync(
            "테스트 페이지를 만들어줘",
            "default",
            CancellationToken.None);

        // Assert - API 키가 없으면 실패해야 함
        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.NotEmpty(result.ErrorMessage);
    }

    [Fact]
    public async Task GeneratePageAsync_WithCancelledToken_ThrowsOrReturnsFailure()
    {
        // Arrange
        var service = CreateService();
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        // Act
        var result = await service.GeneratePageAsync(
            "테스트 페이지",
            "tenant-a",
            cts.Token);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.Success);
    }
}
