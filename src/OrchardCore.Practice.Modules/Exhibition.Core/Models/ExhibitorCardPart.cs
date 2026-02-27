using OrchardCore.ContentManagement;

namespace Exhibition.Core.Models;

public class ExhibitorCardPart : ContentPart
{
    public string LogoUrl { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string WebsiteUrl { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
}
