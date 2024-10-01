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
        await ConnectToDatabase();
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
                this.logger.LogInformation("Connected to Cosmos DB");
                break;
            }
            catch (HttpRequestException ex) when (ex.HttpRequestError is HttpRequestError.SecureConnectionError)
            {
                this.logger.LogWarning(ex, "Failed to connect to Cosmos DB. Retrying in 1 seconds.");
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
                    this.logger.LogInformation("Created database {DatabaseName}", databaseName);
                    return createDatabaseResponse.Database;
                }
            }
            catch (CosmosException ex)
            {
                this.logger.LogWarning(ex, "Error creating database {DatabaseName}. Retrying in 1 seconds.", databaseName);
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
                    this.logger.LogInformation("Created container {ContainerName}", containerName);
                }

                break;
            }
            catch (CosmosException ex)
            {
                this.logger.LogWarning(ex, "Error creating container {ContainerName}. Retrying in 1 seconds.", containerName);
                await Task.Delay(1000);
            }
        }
    }
}
