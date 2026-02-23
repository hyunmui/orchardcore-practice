using Exhibition.Core.Models;

namespace Exhibition.Tests.Models;

public class ExhibitionCoreModelsTests
{
    [Fact]
    public void HeroBannerPart_DefaultValues_AreCorrect()
    {
        var part = new HeroBannerPart();

        Assert.Equal(string.Empty, part.Subtitle);
        Assert.Equal(string.Empty, part.CtaText);
        Assert.Equal(string.Empty, part.CtaUrl);
        Assert.Equal(string.Empty, part.BackgroundImageUrl);
    }

    [Fact]
    public void ExhibitorCardPart_DefaultValues_AreCorrect()
    {
        var part = new ExhibitorCardPart();

        Assert.Equal(string.Empty, part.LogoUrl);
        Assert.Equal(string.Empty, part.CompanyName);
        Assert.Equal(string.Empty, part.Description);
        Assert.Equal(string.Empty, part.WebsiteUrl);
        Assert.Equal(string.Empty, part.Category);
    }

    [Fact]
    public void ProductGalleryPart_DefaultColumns_IsThree()
    {
        var part = new ProductGalleryPart();

        Assert.Equal(3, part.Columns);
        Assert.Empty(part.Items);
    }

    [Fact]
    public void ProductGalleryPart_CanAddItems()
    {
        var part = new ProductGalleryPart();
        part.Items.Add(new GalleryItem
        {
            ImageUrl = "https://example.com/product1.jpg",
            Caption = "제품 1",
            LinkUrl = "/products/1"
        });

        Assert.Single(part.Items);
        Assert.Equal("제품 1", part.Items[0].Caption);
    }

    [Fact]
    public void SchedulePart_DefaultItems_IsEmpty()
    {
        var part = new SchedulePart();

        Assert.Empty(part.Items);
    }

    [Fact]
    public void SchedulePart_CanAddScheduleItems()
    {
        var part = new SchedulePart();
        var now = DateTime.UtcNow;
        part.Items.Add(new ScheduleItem
        {
            Title = "개막식",
            StartTime = now,
            EndTime = now.AddHours(2),
            Location = "메인 홀",
            Speaker = "홍길동"
        });

        Assert.Single(part.Items);
        Assert.Equal("개막식", part.Items[0].Title);
        Assert.Equal("메인 홀", part.Items[0].Location);
    }

    [Fact]
    public void ContactFormPart_DefaultFormTitle_IsKorean()
    {
        var part = new ContactFormPart();

        Assert.Equal("문의하기", part.FormTitle);
        Assert.NotEmpty(part.SuccessMessage);
    }

    [Fact]
    public void GalleryItem_DefaultValues_AreCorrect()
    {
        var item = new GalleryItem();

        Assert.Equal(string.Empty, item.ImageUrl);
        Assert.Equal(string.Empty, item.Caption);
        Assert.Equal(string.Empty, item.LinkUrl);
    }

    [Fact]
    public void ScheduleItem_DefaultValues_AreCorrect()
    {
        var item = new ScheduleItem();

        Assert.Equal(string.Empty, item.Title);
        Assert.Equal(string.Empty, item.Location);
        Assert.Equal(string.Empty, item.Speaker);
        Assert.Equal(string.Empty, item.Description);
    }
}
