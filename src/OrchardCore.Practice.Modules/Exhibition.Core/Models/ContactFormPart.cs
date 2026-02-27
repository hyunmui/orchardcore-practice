using OrchardCore.ContentManagement;

namespace Exhibition.Core.Models;

public class ContactFormPart : ContentPart
{
    public string FormTitle { get; set; } = "문의하기";
    public string SuccessMessage { get; set; } = "문의가 접수되었습니다. 감사합니다.";
    public string RecipientEmail { get; set; } = string.Empty;
}
