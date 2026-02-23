using Exhibition.Core.Models;

namespace Exhibition.Core.ViewModels;

public class ProductGalleryPartViewModel
{
    public int Columns { get; set; } = 3;
    public List<GalleryItem> Items { get; set; } = [];
}
