using Integration.Api.Catalog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IArticleCatalog, InMemoryArticleCatalog>();

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "healthy" }))
   .WithName("Health");

app.MapGet("/api/articles", (IArticleCatalog catalog) =>
    Results.Ok(catalog.GetAll()))
   .WithName("ListArticles");

app.MapGet("/api/articles/{id:int}", (int id, IArticleCatalog catalog) =>
    catalog.Find(id) is { } article
        ? Results.Ok(article)
        : Results.NotFound(new { message = "Article not found" }))
   .WithName("GetArticle");

app.Run();

// Expuesto para WebApplicationFactory en los tests de integración HTTP.
public partial class Program { }
