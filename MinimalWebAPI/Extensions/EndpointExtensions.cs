using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MinimalWebAPI.Data;
using MinimalWebAPI.Models;
using MinimalWebAPI.Models.APIModels;
using MinimalWebAPI.Models.Enums;

namespace MinimalWebAPI.Extensions;

public static class EndpointExtensions
{

    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/items", async (ApiDbContext context, IMapper mapper) =>
        {
            var items = await context.Items.ToListAsync();
            return Results.Ok(mapper.Map<IEnumerable<ItemAPIModel>>(items));
        })
        .WithName("GetAllItems")
        .WithOpenApi();

        app.MapGet("/items/small", async (ApiDbContext context, IMapper mapper) =>
        {
            var items = await context.Items.Where(i => i.Size == ItemSize.small).ToListAsync();
            return Results.Ok(mapper.Map<IEnumerable<ItemAPIModel>>(items));
        })
        .WithName("GetAllSmallItems")
        .WithOpenApi();

        app.MapGet("/items/medium", async (ApiDbContext context, IMapper mapper) =>
        {
            var items = await context.Items.Where(i => i.Size == ItemSize.medium).ToListAsync();
            return Results.Ok(mapper.Map<IEnumerable<ItemAPIModel>>(items));
        })
        .WithName("GetAllMediumItems")
        .WithOpenApi();

        app.MapGet("/items/large", async (ApiDbContext context, IMapper mapper) =>
        {
            var items = await context.Items.Where(i => i.Size == ItemSize.large).ToListAsync();
            return Results.Ok(mapper.Map<IEnumerable<ItemAPIModel>>(items));
        })
        .WithName("GetAllLargeItems")
        .WithOpenApi();

        app.MapGet("/items/{id}", async (int id, ApiDbContext context, IMapper mapper) =>
                                            await context.Items.FindAsync(id)
                                            is Item item
                                            ? Results.Ok(mapper.Map<ItemAPIModel>(item))
                                            : Results.NotFound())
                                            .WithName("GetItemById")
                                            .WithOpenApi();

        app.MapPost("/items", async (ItemAPIModel itemToCreate, ApiDbContext context, IMapper mapper) =>
        {
            var createdItem = context.Items.Add(mapper.Map<Item>(itemToCreate));
            await context.SaveChangesAsync();

            return Results.Ok(createdItem);
        })
        .WithName("CreateItem")
        .WithOpenApi();

        app.MapPut("/items/{id}", async (int id, ItemAPIModel itemToUpdate, ApiDbContext context) =>
        {
            var item = await context.Items.FindAsync(id);

            if (item is null) return Results.NotFound();

            item.Name = itemToUpdate.Name;
            item.Size = itemToUpdate.Size;

            await context.SaveChangesAsync();

            return Results.Ok(itemToUpdate);
        })
        .WithName("UpdateItem")
        .WithOpenApi();

        app.MapDelete("/items/{id}", async (int id, ApiDbContext context) =>
        {
            if (await context.Items.FindAsync(id) is Item itemToDelete)
            {
                context.Items.Remove(itemToDelete);
                await context.SaveChangesAsync();
                return Results.NoContent();
            }
            return Results.NotFound();
        })
        .WithName("DeleteItem")
        .WithOpenApi();

        return app;
    }
}