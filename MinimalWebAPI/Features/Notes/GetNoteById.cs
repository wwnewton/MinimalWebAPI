// <copyright file="GetNoteById.cs" company="Newton Software">
// Copyright (c) Newton Software. All rights reserved.
// </copyright>

namespace MinimalWebAPI.Features.Notes;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using MinimalWebAPI.Infrastructure.Endpoints;
using MinimalWebAPI.Infrastructure.Persistence;

/// <summary>
/// Get note by id endpoint.
/// </summary>
public class GetNoteById : IEndpoint
{
    /// <inheritdoc/>
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapGet("/{id}", Handle)
           .WithName("GetNoteById")
           .WithSummary("Get note by id.");

    private static async Task<Results<Ok<Note>, NotFound>> Handle([AsParameters] GetNoteByIdRequest request, Repository repository)
    {
        var note = await repository.GetByIdAsync<Note>(request.Id);
        if (note is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(note);
    }

    /// <summary>
    /// Request for get note by id.
    /// </summary>
    /// <param name="Id">Note id.</param>
    public record GetNoteByIdRequest(Guid Id);
}
