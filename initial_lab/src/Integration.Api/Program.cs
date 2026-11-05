using Integration.Api.Catalog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IArticleCatalog, InMemoryArticleCatalog>();

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "healthy" }))
   .WithName("Health");

app.MapGet("/api/articles", (IArticleCatalog catalog) =>
    Results.Ok(catalog.GetAll()))
   .WithName("ListArticles");

app.MapGet("/api/articles/{id:int}", (
    int id,
    IArticleCatalog catalog,
    IWebHostEnvironment environment) =>
{
    var article = catalog.Find(id);

    if (article is null)
        return Results.NotFound(new { message = "Article not found" });

    if (environment.IsProduction() && id == 42) // inyección deliberada de fallo para el laboratorio
        return Results.Ok(article with { PriceEur = 8.90m });

    return Results.Ok(article);
})
.WithName("GetArticle");

app.Run();

// Expuesto para WebApplicationFactory en los tests de integración HTTP.
public partial class Program { }
