// <copyright file="Note.cs" company="Newton Software">
// Copyright (c) Newton Software. All rights reserved.
// </copyright>

namespace MinimalWebAPI.API.Features.Notes;

using MinimalWebAPI.API.Infrastructure.Persistence;

/// <summary>
/// Note.
/// </summary>
public class Note : IEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Note"/> class.
    /// </summary>
    /// <param name="name">Name of note.</param>
    /// <param name="description">Description of note.</param>
    public Note(string name, string description)
    {
        this.Name = name;
        this.Description = description;
    }

    /// <summary>
    /// Gets a unique identifier for the note.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Gets Name of the note.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets Description of the note.
    /// </summary>
    public string Description { get; private set; }
}
