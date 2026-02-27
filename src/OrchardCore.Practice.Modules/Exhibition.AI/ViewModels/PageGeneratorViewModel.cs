using Exhibition.AI.Services;
using System.ComponentModel.DataAnnotations;

namespace Exhibition.AI.ViewModels;

public class PageGeneratorViewModel
{
    [Required(ErrorMessage = "페이지 설명을 입력해주세요.")]
    [Display(Name = "페이지 설명")]
    public string Prompt { get; set; } = string.Empty;

    public PageGenerationResult? GenerationResult { get; set; }
    public bool HasResult => GenerationResult is not null;
    public bool IsSuccess => GenerationResult?.Success ?? false;
}
