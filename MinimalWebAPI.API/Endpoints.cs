// <copyright file="Endpoints.cs" company="Newton Software">
// Copyright (c) Newton Software. All rights reserved.
// </copyright>

namespace MinimalWebAPI;

using MinimalWebAPI.API.Features.Notes;
using MinimalWebAPI.API.Features.TodoItems;
using MinimalWebAPI.API.Infrastructure.Endpoints;

/// <summary>
/// Map endpoint extensions.
/// </summary>
#pragma warning disable CA1724 // Type names should not match namespaces
public static class Endpoints
{
#pragma warning restore CA1724 // Type names should not match namespaces
    /// <summary>
    /// Map all endpoints.
    /// </summary>
    /// <param name="app">Web application.</param>
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapGroup("/todo-items")
           .WithTags("TodoItems")
           .MapEndpoint<CreateTodoItem>()
           .MapEndpoint<GetTodoItemById>()
           .MapEndpoint<GetTodoItems>();

        // Notice that the endpoints are in the order of registration.
        app.MapGroup("/notes")
            .WithTags("Notes")
            .MapEndpoint<GetNoteById>()
            .MapEndpoint<CreateNote>();
    }

    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app)
        where TEndpoint : IEndpoint
    {
        TEndpoint.Map(app);
        return app;
    }
}
