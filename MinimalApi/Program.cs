using Microsoft.EntityFrameworkCore;
using MinimalApi.Data;
using MinimalApi.Models;
using MinimalApi.Models.Enums;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApiDbContext>(opt => opt.UseInMemoryDatabase("ApiData"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "MinimalApi";
    config.Title = "MinimalAPI v1";
    config.Version = "v1";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "MinimalApi";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
    app.MapGet("/",  () => {
       return Results.Redirect("/swagger");
    });
}   


app.MapGet("/items", async (ApiDbContext context) => await context.Items.ToListAsync());
app.MapGet("/items/small", async (ApiDbContext context) => await context.Items.Where(i => i.Size == ItemSize.small).ToListAsync());
app.MapGet("/items/medium", async (ApiDbContext context) => await context.Items.Where(i => i.Size == ItemSize.medium).ToListAsync());
app.MapGet("/items/large", async (ApiDbContext context) => await context.Items.Where(i => i.Size == ItemSize.large).ToListAsync());
app.MapGet("/items/{id}", async (int id, ApiDbContext context) =>
                                    await context.Items.FindAsync(id)
                                    is Item item
                                    ? Results.Ok(item)
                                    : Results.NotFound());

app.MapPost("/items", async (Item itemToCreate, ApiDbContext context) =>
{
    context.Items.Add(itemToCreate);
    await context.SaveChangesAsync();

    return Results.Created($"/items/{itemToCreate.Id}", itemToCreate);
});

app.MapPut("/items/{id}", async (int id, Item itemToUpdate, ApiDbContext context) =>
{
    var item = await context.Items.FindAsync(id);

    if (item is null) return Results.NotFound();

    item.Name = itemToUpdate.Name;
    item.Size = itemToUpdate.Size;

    await context.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/items/{id}", async (int id, ApiDbContext context) =>
{
    if (await context.Items.FindAsync(id) is Item itemToDelete)
    {
        context.Items.Remove(itemToDelete);
        await context.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound();
});

app.Run();