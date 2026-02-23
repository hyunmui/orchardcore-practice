using Exhibition.AI.Services;
using Exhibition.AI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrchardCore.Admin;
using OrchardCore.Modules;

namespace Exhibition.AI.Controllers;

[Admin]
[Feature("Exhibition.AI")]
public class PageGeneratorController(
    IPageGeneratorService pageGeneratorService,
    ILogger<PageGeneratorController> logger) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var viewModel = new PageGeneratorViewModel();
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Generate(PageGeneratorViewModel viewModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View("Index", viewModel);

        var tenantId = HttpContext.Request.Host.Host;

        logger.LogInformation("AI 페이지 생성 요청: {Prompt} (테넌트: {TenantId})", viewModel.Prompt, tenantId);

        viewModel.GenerationResult = await pageGeneratorService.GeneratePageAsync(
            viewModel.Prompt,
            tenantId,
            cancellationToken);

        return View("Index", viewModel);
    }
}
