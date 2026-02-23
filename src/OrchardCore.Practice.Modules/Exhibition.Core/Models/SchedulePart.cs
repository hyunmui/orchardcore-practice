using OrchardCore.ContentManagement;

namespace Exhibition.Core.Models;

public class SchedulePart : ContentPart
{
    public List<ScheduleItem> Items { get; set; } = [];
}

public class ScheduleItem
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Speaker { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
