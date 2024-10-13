using Microsoft.Azure.Cosmos;
using System.Net;

namespace MinimalWebAPI.CosmosInitializer;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> logger;
    private readonly CosmosClient cosmosClient;
    private readonly IHostApplicationLifetime hostApplicationLifetime;

    public Worker(ILogger<Worker> logger, CosmosClient cosmosClient, IHostApplicationLifetime hostApplicationLifetime)
    {
        this.hostApplicationLifetime = hostApplicationLifetime;
        this.cosmosClient = cosmosClient;
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await this.ConnectToDatabase();
        var database = await this.CreateDatabase("todo");
        await this.CreateContainer(database, "todoItem", "/id");
        await this.CreateContainer(database, "note", "/id");
        this.hostApplicationLifetime.StopApplication();
    }

    private async Task ConnectToDatabase()
    {
        // Might be a good idea to refactor this retry logic into a reusable method.
        while (true)
        {
            try
            {
                await this.cosmosClient.ReadAccountAsync();
                this.logger.ConnectedToCosmosDb();
                break;
            }
            catch (HttpRequestException ex) when (ex.HttpRequestError is HttpRequestError.SecureConnectionError)
            {
                this.logger.LogFailedToConnectToCosmosDb(ex);
                await Task.Delay(1000);
            }
        }
    }

    private async Task<Database> CreateDatabase(string databaseName)
    {
        while (true)
        {
            try
            {
                var createDatabaseResponse = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseName);
                if (createDatabaseResponse.StatusCode == HttpStatusCode.Created)
                {
                    this.logger.LogDatabaseCreated(databaseName);
                    return createDatabaseResponse.Database;
                }

                if (createDatabaseResponse.StatusCode == HttpStatusCode.OK)
                {
                    this.logger.LogDatabaseExists(databaseName);
                    return createDatabaseResponse.Database;
                }
            }
            catch (CosmosException ex)
            {
                this.logger.LogErrorCreatingDatabase(ex, databaseName);
                await Task.Delay(1000);
            }
        }
    }

    private async Task CreateContainer(Database database, string containerName, string partitionKey)
    {
        while (true)
        {
            try
            {
                var containerResponse = await database!.CreateContainerIfNotExistsAsync(containerName, partitionKey);
                if (containerResponse.StatusCode == HttpStatusCode.Created)
                {
                    this.logger.LogContainerCreated(containerName);
                }

                break;
            }
            catch (CosmosException ex)
            {
                this.logger.LogErrorCreatingContainer(ex, containerName);
                await Task.Delay(1000);
            }
        }
    }
}

public static partial class Log
{
    [LoggerMessage(EventId = 0, Level = LogLevel.Information, Message = "Connected to Cosmos DB")]
    public static partial void ConnectedToCosmosDb(this ILogger logger);

    [LoggerMessage(EventId = 1, Level = LogLevel.Warning, Message = "Failed to connect to Cosmos DB. Retrying in 1 seconds.")]
    public static partial void LogFailedToConnectToCosmosDb(this ILogger logger, Exception ex);

    [LoggerMessage(EventId = 2, Level = LogLevel.Information, Message = "Created database {DatabaseName}")]
    public static partial void LogDatabaseCreated(this ILogger logger, string databaseName);

    [LoggerMessage(EventId = 3, Level = LogLevel.Information, Message = "Database {DatabaseName} already exists")]
    public static partial void LogDatabaseExists(this ILogger logger, string databaseName);

    [LoggerMessage(EventId = 4, Level = LogLevel.Warning, Message = "Error creating database {DatabaseName}. Retrying in 1 seconds.")]
    public static partial void LogErrorCreatingDatabase(this ILogger logger, Exception ex, string databaseName);

    [LoggerMessage(EventId = 5, Level = LogLevel.Information, Message = "Created container {ContainerName}")]
    public static partial void LogContainerCreated(this ILogger logger, string containerName);

    [LoggerMessage(EventId = 6, Level = LogLevel.Warning, Message = "Error creating container {ContainerName}. Retrying in 1 seconds.")]
    public static partial void LogErrorCreatingContainer(this ILogger logger, Exception ex, string containerName);
}
