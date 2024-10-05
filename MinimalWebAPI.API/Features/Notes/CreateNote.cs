﻿// <copyright file="CreateNote.cs" company="Newton Software">
// Copyright (c) Newton Software. All rights reserved.
// </copyright>

namespace MinimalWebAPI.API.Features.Notes;

using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using MinimalWebAPI.API.Infrastructure.Endpoints;
using MinimalWebAPI.API.Infrastructure.Persistence;
using MinimalWebAPI.API.Infrastructure.Validation;
using MinimalWebAPI.Shared.Notes;

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

    private static async Task<CreatedAtRoute<Note>> Handle(CreateNoteRequest command, Repository repository, IBus bus, CancellationToken cancellationToken)
    {
        var note = new Note(command.Name, command.Description);

        // This could end up saving the note but not sending the service bus message.
        await repository.AddAsync(note, cancellationToken);
        await bus.Send(new NoteCreated(note.Id, note.Name, note.Description), cancellationToken);
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
