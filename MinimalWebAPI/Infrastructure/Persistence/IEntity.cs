// <copyright file="IEntity.cs" company="Newton Software">
// Copyright (c) Newton Software. All rights reserved.
// </copyright>

namespace MinimalWebAPI.Infrastructure.Persistence;

/// <summary>
/// Entity types.
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Gets entity Id.
    /// </summary>
    Guid Id { get; init; }
}
