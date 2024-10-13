// <copyright file="NoteConsumer.cs" company="Newton Software">
// Copyright (c) Newton Software. All rights reserved.
// </copyright>

namespace MinimalWebAPI.Functions;

using MassTransit;
using Microsoft.Extensions.Logging;
using MinimalWebAPI.Shared.Notes;

/// <summary>
/// Note consumer.
/// </summary>
public class NoteConsumer : IConsumer<NoteCreated>
{
    private readonly ILogger<NoteConsumer> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="NoteConsumer"/> class.
    /// </summary>
    /// <param name="logger">Logger.</param>
    public NoteConsumer(ILogger<NoteConsumer> logger)
    {
        this.logger = logger;
    }

    /// <inheritdoc/>
    public Task Consume(ConsumeContext<NoteCreated> context)
    {
        ArgumentNullException.ThrowIfNull(context);
        this.logger.LogInformation("Received note created: {Id}", context.Message.Id);
        return Task.CompletedTask;
    }
}
