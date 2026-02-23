using Exhibition.Core.Models;
using Exhibition.Core.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;

namespace Exhibition.Core.Drivers;

public class SchedulePartDisplayDriver : ContentPartDisplayDriver<SchedulePart>
{
    public override IDisplayResult Display(SchedulePart part, BuildPartDisplayContext context)
    {
        return Initialize<SchedulePartViewModel>(
            $"{nameof(SchedulePart)}__{context.DisplayType}",
            viewModel =>
            {
                viewModel.Items = part.Items;
            });
    }

    public override IDisplayResult Edit(SchedulePart part, BuildPartEditorContext context)
    {
        return Initialize<SchedulePartViewModel>(
            $"{nameof(SchedulePart)}_Edit",
            viewModel =>
            {
                viewModel.Items = part.Items;
            });
    }

    public override async Task<IDisplayResult> UpdateAsync(
        SchedulePart part,
        UpdatePartEditorContext context)
    {
        var viewModel = new SchedulePartViewModel();
        await context.Updater.TryUpdateModelAsync(viewModel, Prefix);

        part.Items = viewModel.Items;

        return Edit(part, context);
    }
}
