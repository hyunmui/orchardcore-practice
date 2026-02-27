using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace Exhibition.AI.Services;

/// <summary>
/// Semantic Kernel을 이용하여 Claude API로 페이지를 생성하는 서비스
/// </summary>
public class PageGeneratorService(
    ILogger<PageGeneratorService> logger,
    IOptions<AiOptions> aiOptions) : IPageGeneratorService
{
    private readonly AiOptions _options = aiOptions.Value;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task<PageGenerationResult> GeneratePageAsync(
        string naturalLanguagePrompt,
        string tenantId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var systemPrompt = await LoadSystemPromptAsync(cancellationToken);
            var kernel = BuildKernel();

            var chatCompletion = kernel.GetRequiredService<IChatCompletionService>();

            var history = new ChatHistory();
            history.AddSystemMessage(systemPrompt);
            history.AddUserMessage($"테넌트 ID: {tenantId}\n\n요청: {naturalLanguagePrompt}");

            var executionSettings = new OpenAIPromptExecutionSettings
            {
                MaxTokens = 4096,
                Temperature = 0.7
            };

            var response = await chatCompletion.GetChatMessageContentAsync(
                history,
                executionSettings,
                kernel,
                cancellationToken);

            var jsonContent = ExtractJson(response.Content ?? string.Empty);

            var result = JsonSerializer.Deserialize<PageGenerationResult>(jsonContent, JsonOptions);
            if (result is null)
            {
                return new PageGenerationResult
                {
                    Success = false,
                    ErrorMessage = "AI 응답을 파싱할 수 없습니다."
                };
            }

            result.Success = true;
            logger.LogInformation("페이지 생성 완료: {Title} (테넌트: {TenantId})", result.Title, tenantId);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "페이지 생성 중 오류 발생 (테넌트: {TenantId})", tenantId);
            return new PageGenerationResult
            {
                Success = false,
                ErrorMessage = $"페이지 생성에 실패했습니다: {ex.Message}"
            };
        }
    }

    private Kernel BuildKernel()
    {
        var builder = Kernel.CreateBuilder();

        // Claude API는 OpenAI 호환 엔드포인트를 사용
        builder.AddOpenAIChatCompletion(
            modelId: _options.ModelId,
            apiKey: _options.ApiKey,
            httpClient: CreateAnthropicHttpClient());

        return builder.Build();
    }

    private HttpClient CreateAnthropicHttpClient()
    {
        var handler = new AnthropicHttpClientHandler(_options.Endpoint);
        return new HttpClient(handler);
    }

    private async Task<string> LoadSystemPromptAsync(CancellationToken cancellationToken)
    {
        var promptPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Prompts",
            "PageGenerator.txt");

        if (!File.Exists(promptPath))
        {
            // 모듈 디렉토리에서 검색
            var moduleDir = Path.GetDirectoryName(typeof(PageGeneratorService).Assembly.Location)!;
            promptPath = Path.Combine(moduleDir, "Prompts", "PageGenerator.txt");
        }

        if (!File.Exists(promptPath))
        {
            logger.LogWarning("PageGenerator.txt 프롬프트 파일을 찾을 수 없습니다. 기본 프롬프트를 사용합니다.");
            return GetDefaultSystemPrompt();
        }

        return await File.ReadAllTextAsync(promptPath, cancellationToken);
    }

    private static string GetDefaultSystemPrompt() =>
        "당신은 전시회 웹사이트 페이지 생성 AI입니다. JSON 형식으로만 응답하세요.";

    private static string ExtractJson(string content)
    {
        // 마크다운 코드 블록 제거
        var start = content.IndexOf('{');
        var end = content.LastIndexOf('}');

        if (start >= 0 && end > start)
            return content[start..(end + 1)];

        return content;
    }
}

/// <summary>
/// Anthropic API 요청 헤더를 추가하는 HTTP 핸들러
/// </summary>
internal sealed class AnthropicHttpClientHandler(string endpoint) : HttpClientHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        // Anthropic API 전용 헤더 추가
        request.Headers.TryAddWithoutValidation("anthropic-version", "2023-06-01");

        // 엔드포인트 재설정 (OpenAI 호환 경로로 변환)
        if (request.RequestUri is not null)
        {
            var path = request.RequestUri.PathAndQuery;
            request.RequestUri = new Uri($"{endpoint.TrimEnd('/')}{path}");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
