using Exhibition.Core.Models;
using Exhibition.Core.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;

namespace Exhibition.Core.Drivers;

public class ExhibitorCardPartDisplayDriver : ContentPartDisplayDriver<ExhibitorCardPart>
{
    public override IDisplayResult Display(ExhibitorCardPart part, BuildPartDisplayContext context)
    {
        return Initialize<ExhibitorCardPartViewModel>(
            $"{nameof(ExhibitorCardPart)}__{context.DisplayType}",
            viewModel =>
            {
                viewModel.LogoUrl = part.LogoUrl;
                viewModel.CompanyName = part.CompanyName;
                viewModel.Description = part.Description;
                viewModel.WebsiteUrl = part.WebsiteUrl;
                viewModel.Category = part.Category;
            });
    }

    public override IDisplayResult Edit(ExhibitorCardPart part, BuildPartEditorContext context)
    {
        return Initialize<ExhibitorCardPartViewModel>(
            $"{nameof(ExhibitorCardPart)}_Edit",
            viewModel =>
            {
                viewModel.LogoUrl = part.LogoUrl;
                viewModel.CompanyName = part.CompanyName;
                viewModel.Description = part.Description;
                viewModel.WebsiteUrl = part.WebsiteUrl;
                viewModel.Category = part.Category;
            });
    }

    public override async Task<IDisplayResult> UpdateAsync(
        ExhibitorCardPart part,
        UpdatePartEditorContext context)
    {
        var viewModel = new ExhibitorCardPartViewModel();
        await context.Updater.TryUpdateModelAsync(viewModel, Prefix);

        part.LogoUrl = viewModel.LogoUrl;
        part.CompanyName = viewModel.CompanyName;
        part.Description = viewModel.Description;
        part.WebsiteUrl = viewModel.WebsiteUrl;
        part.Category = viewModel.Category;

        return Edit(part, context);
    }
}
