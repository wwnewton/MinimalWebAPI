// <copyright file="GetTodoItemById.cs" company="Newton Software">
// Copyright (c) Newton Software. All rights reserved.
// </copyright>

namespace MinimalWebAPI.Features.TodoItems;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using MinimalWebAPI.Infrastructure.Endpoints;
using MinimalWebAPI.Infrastructure.Persistence;

/// <summary>
/// Get todoItem by id.
/// </summary>
public class GetTodoItemById : IEndpoint
{
    /// <inheritdoc/>
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/{id}", Handle)
           .WithName("GetTodoItemById")
           .WithSummary("Get todo item by id.");
    }

    private static async Task<Results<Ok<TodoItem>, NotFound>> Handle([AsParameters] GetTodoItemByIdRequest request, Repository repository)
    {
        var todoItem = await repository.GetByIdAsync<TodoItem>(request.Id);
        if (todoItem is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(todoItem);
    }

    /// <summary>
    /// Request for get TodoItem by id.
    /// </summary>
    /// <param name="Id">TodoItem Id.</param>
    public record GetTodoItemByIdRequest(Guid Id);
}
