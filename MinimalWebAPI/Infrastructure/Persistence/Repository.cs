// <copyright file="Repository.cs" company="Newton Software">
// Copyright (c) Newton Software. All rights reserved.
// </copyright>

namespace MinimalWebAPI.Infrastructure.Persistence;

using Microsoft.Azure.Cosmos;

/// <summary>
/// Generic repository.
/// </summary>
public class Repository
{
    private readonly Database cosmosDb;

    /// <summary>
    /// Initializes a new instance of the <see cref="Repository"/> class.
    /// </summary>
    /// <param name="cosmosClient">Cosmos db client.</param>
    public Repository(CosmosClient cosmosClient)
    {
        this.cosmosDb = cosmosClient.GetDatabase("todo");
    }

    /// <summary>
    /// Add Item to repository.
    /// </summary>
    /// <typeparam name="T">Type of entity.</typeparam>
    /// <param name="entity">entity.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task AddAsync<T>(T entity)
        where T : IEntity
    {
        var container = this.cosmosDb.GetContainer(GetContainerName<T>());
        await container.CreateItemAsync(entity);
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
        var container = this.cosmosDb.GetContainer(GetContainerName<T>());
        return await container.ReadItemAsync<T>(id.ToString(), new PartitionKey(id.ToString()));
    }

    private static string GetContainerName<T>()
    {
        var containereName = typeof(T).Name;
        containereName = char.ToLowerInvariant(containereName[0]) + containereName[1..];
        return containereName;
    }
}
