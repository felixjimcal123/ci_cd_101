namespace Integration.Api.Catalog;

public sealed class InMemoryArticleCatalog : IArticleCatalog
{
    private static readonly IReadOnlyList<Article> Articles = new[]
    {
        new Article(42, "Champú", 7.90m),
        new Article(43, "Crema hidratante", 12.50m),
        new Article(44, "Protector solar", 15.25m)
    };

    public IReadOnlyList<Article> GetAll() => Articles;

    public Article? Find(int id) => Articles.FirstOrDefault(a => a.Id == id);
}
