using Exhibition.Core.Models;
using Exhibition.Core.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;

namespace Exhibition.Core.Drivers;

public class ContactFormPartDisplayDriver : ContentPartDisplayDriver<ContactFormPart>
{
    public override IDisplayResult Display(ContactFormPart part, BuildPartDisplayContext context)
    {
        return Initialize<ContactFormPartViewModel>(
            $"{nameof(ContactFormPart)}__{context.DisplayType}",
            viewModel =>
            {
                viewModel.FormTitle = part.FormTitle;
                viewModel.SuccessMessage = part.SuccessMessage;
            });
    }

    public override IDisplayResult Edit(ContactFormPart part, BuildPartEditorContext context)
    {
        return Initialize<ContactFormPartViewModel>(
            $"{nameof(ContactFormPart)}_Edit",
            viewModel =>
            {
                viewModel.FormTitle = part.FormTitle;
                viewModel.SuccessMessage = part.SuccessMessage;
            });
    }

    public override async Task<IDisplayResult> UpdateAsync(
        ContactFormPart part,
        UpdatePartEditorContext context)
    {
        var viewModel = new ContactFormPartViewModel();
        await context.Updater.TryUpdateModelAsync(viewModel, Prefix);

        part.FormTitle = viewModel.FormTitle;
        part.SuccessMessage = viewModel.SuccessMessage;

        return Edit(part, context);
    }
}
