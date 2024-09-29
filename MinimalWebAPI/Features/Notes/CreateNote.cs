// <copyright file="CreateNote.cs" company="Newton Software">
// Copyright (c) Newton Software. All rights reserved.
// </copyright>

namespace MinimalWebAPI.Features.Notes;

using System;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.Cosmos;
using MinimalWebAPI.Features.TodoItems;
using MinimalWebAPI.Infrastructure.Endpoints;
using MinimalWebAPI.Infrastructure.Persistence;
using MinimalWebAPI.Infrastructure.Validation;

/// <summary>
/// Create note endpoint.
/// </summary>
public class CreateNote : IEndpoint
{
    /// <inheritdoc/>
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapPost("/", Handle)
           .WithSummary("Create note.")
           .WithRequestValidation<CreateNoteRequest>();

    private static async Task<CreatedAtRoute<Note>> Handle(CreateNoteRequest command, Repository repository)
    {
        var note = new Note(command.Name, command.Description);
        await repository.AddAsync(note);
        return TypedResults.CreatedAtRoute(note, "GetNoteById", new { note.Id });
    }

    /// <summary>
    /// Request for create note.
    /// </summary>
    /// <param name="Name">Name of note.</param>
    /// <param name="Description">Description of note.</param>
    public record CreateNoteRequest(string Name, string Description);

    /// <summary>
    /// Request validator.
    /// </summary>
    public class RequestValidator : AbstractValidator<CreateNoteRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestValidator"/> class.
        /// </summary>
        public RequestValidator()
        {
            this.RuleFor(x => x.Name).NotEmpty();
            this.RuleFor(x => x.Description).NotEmpty();
        }
    }
}
