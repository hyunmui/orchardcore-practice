using Exhibition.Core.Models;
using Exhibition.Core.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;

namespace Exhibition.Core.Drivers;

public class ProductGalleryPartDisplayDriver : ContentPartDisplayDriver<ProductGalleryPart>
{
    public override IDisplayResult Display(ProductGalleryPart part, BuildPartDisplayContext context)
    {
        return Initialize<ProductGalleryPartViewModel>(
            $"{nameof(ProductGalleryPart)}__{context.DisplayType}",
            viewModel =>
            {
                viewModel.Columns = part.Columns;
                viewModel.Items = part.Items;
            });
    }

    public override IDisplayResult Edit(ProductGalleryPart part, BuildPartEditorContext context)
    {
        return Initialize<ProductGalleryPartViewModel>(
            $"{nameof(ProductGalleryPart)}_Edit",
            viewModel =>
            {
                viewModel.Columns = part.Columns;
                viewModel.Items = part.Items;
            });
    }

    public override async Task<IDisplayResult> UpdateAsync(
        ProductGalleryPart part,
        UpdatePartEditorContext context)
    {
        var viewModel = new ProductGalleryPartViewModel();
        await context.Updater.TryUpdateModelAsync(viewModel, Prefix);

        part.Columns = viewModel.Columns;
        part.Items = viewModel.Items;

        return Edit(part, context);
    }
}
