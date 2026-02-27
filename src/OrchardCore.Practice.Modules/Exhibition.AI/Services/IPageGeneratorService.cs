namespace Exhibition.AI.Services;

/// <summary>
/// AI를 이용하여 자연어 프롬프트로 페이지를 생성하는 서비스 인터페이스
/// </summary>
public interface IPageGeneratorService
{
    Task<PageGenerationResult> GeneratePageAsync(
        string naturalLanguagePrompt,
        string tenantId,
        CancellationToken cancellationToken = default);
}

public class PageGenerationResult
{
    public string Title { get; set; } = string.Empty;
    public string Permalink { get; set; } = string.Empty;
    public List<WidgetDefinition> Widgets { get; set; } = [];
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
}

public class WidgetDefinition
{
    public string WidgetType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public Dictionary<string, object> Properties { get; set; } = [];
}
