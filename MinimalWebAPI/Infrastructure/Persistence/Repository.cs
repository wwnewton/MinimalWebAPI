// <copyright file="Repository.cs" company="Newton Software">
// Copyright (c) Newton Software. All rights reserved.
// </copyright>

namespace MinimalWebAPI.Infrastructure.Persistence;

/// <summary>
/// Generic repository.
/// </summary>
public class Repository
{
    private readonly List<object> items = new();

    /// <summary>
    /// Add Item to repository.
    /// </summary>
    /// <typeparam name="T">Type of entity.</typeparam>
    /// <param name="entity">entity.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task AddAsync<T>(T entity)
        where T : IEntity
    {
        this.items.Add(entity);
        await Task.CompletedTask;
    }

    /// <summary>
    /// Get Item by Id.
    /// </summary>
    /// <typeparam name="T">Type of entity.</typeparam>
    /// <param name="id">Id of entity.</param>
    /// <returns>An entity of type T.</returns>
    public async Task<T?> GetByIdAsync<T>(Guid id)
        where T : IEntity
    {
        return await Task.FromResult(this.items.OfType<T>().FirstOrDefault(x => x.Id == id));
    }
}
