// <copyright file="Endpoints.cs" company="Newton Software">
// Copyright (c) Newton Software. All rights reserved.
// </copyright>

namespace MinimalWebAPI;

using MinimalWebAPI.Features.Notes;
using MinimalWebAPI.Features.TodoItems;
using MinimalWebAPI.Infrastructure.Endpoints;

/// <summary>
/// Map endpoint extensions.
/// </summary>
public static class Endpoints
{
    /// <summary>
    /// Map all endpoints.
    /// </summary>
    /// <param name="app">Web application.</param>
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapGroup("/todo-items")
           .WithTags("TodoItems")
           .MapEndpoint<CreateTodoItem>()
           .MapEndpoint<GetTodoItemById>();

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
