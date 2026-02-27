using Exhibition.AI.Services;

namespace Exhibition.Tests.Models;

public class PageGenerationResultTests
{
    [Fact]
    public void PageGenerationResult_DefaultValues_AreCorrect()
    {
        // Arrange & Act
        var result = new PageGenerationResult();

        // Assert
        Assert.Equal(string.Empty, result.Title);
        Assert.Equal(string.Empty, result.Permalink);
        Assert.Equal(string.Empty, result.ErrorMessage);
        Assert.Empty(result.Widgets);
        Assert.False(result.Success);
    }

    [Fact]
    public void PageGenerationResult_WithWidgets_CanBeCreated()
    {
        // Arrange
        var widgets = new List<WidgetDefinition>
        {
            new()
            {
                WidgetType = "HeroBannerWidget",
                Title = "메인 배너",
                Properties = new Dictionary<string, object>
                {
                    { "subtitle", "2024 전시회에 오신 것을 환영합니다" },
                    { "ctaText", "참가 신청" },
                    { "ctaUrl", "/register" }
                }
            },
            new()
            {
                WidgetType = "ContactFormWidget",
                Title = "문의하기",
                Properties = new Dictionary<string, object>
                {
                    { "formTitle", "문의하기" },
                    { "successMessage", "문의가 접수되었습니다." }
                }
            }
        };

        // Act
        var result = new PageGenerationResult
        {
            Title = "2024 전시회 메인 페이지",
            Permalink = "2024-exhibition-main",
            Widgets = widgets,
            Success = true
        };

        // Assert
        Assert.Equal("2024 전시회 메인 페이지", result.Title);
        Assert.Equal("2024-exhibition-main", result.Permalink);
        Assert.Equal(2, result.Widgets.Count);
        Assert.True(result.Success);
        Assert.Equal("HeroBannerWidget", result.Widgets[0].WidgetType);
    }

    [Fact]
    public void WidgetDefinition_DefaultValues_AreCorrect()
    {
        // Arrange & Act
        var widget = new WidgetDefinition();

        // Assert
        Assert.Equal(string.Empty, widget.WidgetType);
        Assert.Equal(string.Empty, widget.Title);
        Assert.Empty(widget.Properties);
    }

    [Fact]
    public void WidgetDefinition_CanStoreArbitraryProperties()
    {
        // Arrange
        var widget = new WidgetDefinition
        {
            WidgetType = "ProductGalleryWidget",
            Properties = new Dictionary<string, object>
            {
                { "columns", 3 },
                { "items", new[] { "item1", "item2" } }
            }
        };

        // Act & Assert
        Assert.Equal(2, widget.Properties.Count);
        Assert.Equal(3, widget.Properties["columns"]);
    }
}
