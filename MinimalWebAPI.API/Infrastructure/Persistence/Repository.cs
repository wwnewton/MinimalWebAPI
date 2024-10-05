// <copyright file="Repository.cs" company="Newton Software">
// Copyright (c) Newton Software. All rights reserved.
// </copyright>

namespace MinimalWebAPI.API.Infrastructure.Persistence;

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
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task AddAsync<T>(T entity, CancellationToken cancellationToken)
        where T : IEntity
    {
        var container = this.cosmosDb.GetContainer(GetContainerName<T>());
        await container.CreateItemAsync(entity, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Get Item by Id.
    /// </summary>
    /// <typeparam name="T">Type of entity.</typeparam>
    /// <param name="id">Id of entity.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An entity of type T.</returns>
    public async Task<T?> GetByIdAsync<T>(Guid id, CancellationToken cancellationToken)
        where T : IEntity
    {
        var container = this.cosmosDb.GetContainer(GetContainerName<T>());
        return await container.ReadItemAsync<T>(id.ToString(), new PartitionKey(id.ToString()), cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Get all items of type T.
    /// </summary>
    /// <typeparam name="T">Type of entity.</typeparam>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task<IEnumerable<T>> GetAllAsync<T>(CancellationToken cancellationToken)
        where T : IEntity
    {
        var container = this.cosmosDb.GetContainer(GetContainerName<T>());
        var query = container.GetItemQueryIterator<T>();
        var results = new List<T>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync(cancellationToken);
            results.AddRange(response);
        }

        return results;
    }

    private static string GetContainerName<T>()
    {
        var containereName = typeof(T).Name;
        containereName = char.ToLowerInvariant(containereName[0]) + containereName[1..];
        return containereName;
    }
}
