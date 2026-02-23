using Exhibition.Core.Models;
using Exhibition.Core.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;

namespace Exhibition.Core.Drivers;

public class HeroBannerPartDisplayDriver : ContentPartDisplayDriver<HeroBannerPart>
{
    public override IDisplayResult Display(HeroBannerPart part, BuildPartDisplayContext context)
    {
        return Initialize<HeroBannerPartViewModel>(
            $"{nameof(HeroBannerPart)}__{context.DisplayType}",
            viewModel =>
            {
                viewModel.Title = part.ContentItem.DisplayText;
                viewModel.Subtitle = part.Subtitle;
                viewModel.CtaText = part.CtaText;
                viewModel.CtaUrl = part.CtaUrl;
                viewModel.BackgroundImageUrl = part.BackgroundImageUrl;
            });
    }

    public override IDisplayResult Edit(HeroBannerPart part, BuildPartEditorContext context)
    {
        return Initialize<HeroBannerPartViewModel>(
            $"{nameof(HeroBannerPart)}_Edit",
            viewModel =>
            {
                viewModel.Subtitle = part.Subtitle;
                viewModel.CtaText = part.CtaText;
                viewModel.CtaUrl = part.CtaUrl;
                viewModel.BackgroundImageUrl = part.BackgroundImageUrl;
            });
    }

    public override async Task<IDisplayResult> UpdateAsync(
        HeroBannerPart part,
        UpdatePartEditorContext context)
    {
        var viewModel = new HeroBannerPartViewModel();
        await context.Updater.TryUpdateModelAsync(viewModel, Prefix);

        part.Subtitle = viewModel.Subtitle;
        part.CtaText = viewModel.CtaText;
        part.CtaUrl = viewModel.CtaUrl;
        part.BackgroundImageUrl = viewModel.BackgroundImageUrl;

        return Edit(part, context);
    }
}
