using Integration.Api.Catalog;

namespace Integration.Api.Tests;

public class ArticleCatalogTests
{
    private readonly InMemoryArticleCatalog _catalog = new();

    [Fact]
    public void Find_KnownArticle_ReturnsExpectedPrice()
    {
        var article = _catalog.Find(42);
        Assert.NotNull(article);
        Assert.Equal(7.90m, article.PriceEur);
    }

    [Fact]
    public void Find_UnknownArticle_ReturnsNull()
    {
        Assert.Null(_catalog.Find(999));
    }
}
