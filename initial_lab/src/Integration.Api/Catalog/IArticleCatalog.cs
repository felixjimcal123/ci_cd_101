namespace Integration.Api.Catalog;

public interface IArticleCatalog
{
    IReadOnlyList<Article> GetAll();
    Article? Find(int id);
}
