// <copyright file="NoteCreated.cs" company="Newton Software">
// Copyright (c) Newton Software. All rights reserved.
// </copyright>

namespace MinimalWebAPI.Shared.Notes;

using System;

/// <summary>
/// Note created event.
/// </summary>
/// <param name="Id">Note id.</param>
/// <param name="Name">Note name.</param>
/// <param name="Description">Note description.</param>
public record NoteCreated(Guid Id, string Name, string Description);