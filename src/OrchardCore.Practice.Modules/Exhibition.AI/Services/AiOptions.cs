namespace Exhibition.AI.Services;

public class AiOptions
{
    public const string SectionName = "AI:Claude";

    public string ModelId { get; set; } = "claude-sonnet-4-6";
    public string ApiKey { get; set; } = string.Empty;
    public string Endpoint { get; set; } = "https://api.anthropic.com";
}
