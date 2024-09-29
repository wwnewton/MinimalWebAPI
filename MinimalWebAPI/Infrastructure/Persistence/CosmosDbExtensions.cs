// <copyright file="CosmosDbExtensions.cs" company="Newton Software">
// Copyright (c) Newton Software. All rights reserved.
// </copyright>

namespace MinimalWebAPI.Infrastructure.Persistence;

using System.Net;
using Microsoft.Azure.Cosmos;

/// <summary>
/// Adds Cosmos DB extensions to WebApplication.
/// </summary>
public static class CosmosDbExtensions
{
    /// <summary>
    /// Creates the database and containers if they do not exist.
    /// </summary>
    /// <param name="app">Web application.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    public static async Task CreateDatabaseAndContainers(this WebApplication app)
    {
        var logger = app.Services.GetRequiredService<ILogger<WebApplication>>();
        logger.LogInformation("Creating database and containers");
        var client = app.Services.GetRequiredService<CosmosClient>();
        await ConnectToDatabase(client, logger);
        var database = await CreateDatabase("todo", client, logger);
        await CreateContainer("todoItem", "/id", database, logger);
        await CreateContainer("note", "/id", database, logger);
    }

    private static async Task ConnectToDatabase(CosmosClient client, ILogger<WebApplication> logger)
    {
        // Might be a good idea to refactor this retry logic into a reusable method.
        while (true)
        {
            try
            {
                await client.ReadAccountAsync();
                logger.LogInformation("Connected to Cosmos DB");
                break;
            }
            catch (HttpRequestException ex) when (ex.HttpRequestError is HttpRequestError.SecureConnectionError)
            {
                logger.LogWarning(ex, "Failed to connect to Cosmos DB. Retrying in 1 seconds.");
                await Task.Delay(1000);
            }
        }
    }

    private static async Task<Database> CreateDatabase(string databaseName, CosmosClient client, ILogger logger)
    {
        while (true)
        {
            try
            {
                var createDatabaseResponse = await client.CreateDatabaseIfNotExistsAsync(databaseName);
                if (createDatabaseResponse.StatusCode == HttpStatusCode.Created)
                {
                    logger.LogInformation("Created database {DatabaseName}", databaseName);
                    return createDatabaseResponse.Database;
                }
            }
            catch (CosmosException ex)
            {
                logger.LogWarning(ex, "Error creating database {DatabaseName}. Retrying in 1 seconds.", databaseName);
                await Task.Delay(1000);
            }
        }
    }

    private static async Task CreateContainer(string containerName, string partitionKey, Database database, ILogger logger)
    {
        while (true)
        {
            try
            {
                var containerResponse = await database.CreateContainerIfNotExistsAsync(containerName, partitionKey);
                if (containerResponse.StatusCode == HttpStatusCode.Created)
                {
                    logger.LogInformation("Created container {ContainerName}", containerName);
                }

                break;
            }
            catch (CosmosException ex)
            {
                logger.LogWarning(ex, "Error creating container {ContainerName}. Retrying in 1 seconds.", containerName);
                await Task.Delay(1000);
            }
        }
    }
}
