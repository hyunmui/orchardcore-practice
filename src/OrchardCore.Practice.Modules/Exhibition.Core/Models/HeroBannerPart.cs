using OrchardCore.ContentManagement;

namespace Exhibition.Core.Models;

public class HeroBannerPart : ContentPart
{
    public string Subtitle { get; set; } = string.Empty;
    public string CtaText { get; set; } = string.Empty;
    public string CtaUrl { get; set; } = string.Empty;
    public string BackgroundImageUrl { get; set; } = string.Empty;
}
