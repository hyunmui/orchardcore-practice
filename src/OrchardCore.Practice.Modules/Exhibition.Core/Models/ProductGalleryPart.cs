using OrchardCore.ContentManagement;

namespace Exhibition.Core.Models;

public class ProductGalleryPart : ContentPart
{
    public int Columns { get; set; } = 3;
    public List<GalleryItem> Items { get; set; } = [];
}

public class GalleryItem
{
    public string ImageUrl { get; set; } = string.Empty;
    public string Caption { get; set; } = string.Empty;
    public string LinkUrl { get; set; } = string.Empty;
}
