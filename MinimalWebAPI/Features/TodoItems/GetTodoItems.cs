// <copyright file="GetTodoItems.cs" company="Newton Software">
// Copyright (c) Newton Software. All rights reserved.
// </copyright>

namespace MinimalWebAPI.Features.TodoItems;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MinimalWebAPI.Infrastructure.Endpoints;
using MinimalWebAPI.Infrastructure.Persistence;

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
           .WithSummary("Get todo item by id.");
    }

    private static async Task<Ok<IEnumerable<TodoItem>>> Handle(Repository repository)
    {
        var todoItems = await repository.GetAllAsync<TodoItem>();
        return TypedResults.Ok(todoItems);
    }
}
