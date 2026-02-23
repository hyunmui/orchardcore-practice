using System.ComponentModel.DataAnnotations;

namespace Exhibition.Core.ViewModels;

public class ContactFormPartViewModel
{
    public string FormTitle { get; set; } = string.Empty;
    public string SuccessMessage { get; set; } = string.Empty;

    [Required(ErrorMessage = "이름을 입력해주세요.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "이메일을 입력해주세요.")]
    [EmailAddress(ErrorMessage = "올바른 이메일 형식이 아닙니다.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "메시지를 입력해주세요.")]
    public string Message { get; set; } = string.Empty;

    public bool IsSubmitted { get; set; }
}
