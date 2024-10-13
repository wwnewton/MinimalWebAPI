// <copyright file="NoteCreated.cs" company="Newton Software">
// Copyright (c) Newton Software. All rights reserved.
// </copyright>

#pragma warning disable CA1716
namespace MinimalWebAPI.Shared.Notes;
#pragma warning restore CA1716
using System;

/// <summary>
/// Note created event.
/// </summary>
/// <param name="Id">Note id.</param>
/// <param name="Name">Note name.</param>
/// <param name="Description">Note description.</param>
public record NoteCreated(Guid Id, string Name, string Description);