// <copyright file="GetTodoItems.cs" company="Newton Software">
// Copyright (c) Newton Software. All rights reserved.
// </copyright>

namespace MinimalWebAPI.API.Features.TodoItems;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Distributed;
using MinimalWebAPI.API.Infrastructure.Caching;
using MinimalWebAPI.API.Infrastructure.Endpoints;
using MinimalWebAPI.API.Infrastructure.Persistence;

/// <summary>
/// Get todoItems.
/// </summary>
public class GetTodoItems : IEndpoint
{
    /// <inheritdoc/>
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/", Handle)
           .WithName("GetTodoItems")
           .WithSummary("Get all todo items.");
    }

    private static async Task<Ok<IEnumerable<TodoItem>>> Handle(Repository repository, IDistributedCache cache)
    {
        var todoItems = await cache.GetOrAddAsync("todoItems", repository.GetAllAsync<TodoItem>);
        return TypedResults.Ok(todoItems);
    }
}
