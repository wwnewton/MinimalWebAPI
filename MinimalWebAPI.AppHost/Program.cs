var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var serviceBus = builder.ExecutionContext.IsPublishMode
    ? builder.AddAzureServiceBus("messaging")
        .AddQueue("test")
    : builder.AddConnectionString("messaging");

var cosmosDb = builder.ExecutionContext.IsPublishMode
    ? builder.AddAzureCosmosDB("cosmos")
        .AddDatabase("todo")
    : builder.AddConnectionString("cosmos");

// This initializes the Cosmos DB database for local testing only.
if (!builder.ExecutionContext.IsPublishMode)
{
    var cosmosInitializer = builder.AddProject<Projects.MinimalWebAPI_CosmosInitializer>("minimalwebapi-cosmosinitializer")
        .WithReference(cosmosDb);
}

var api = builder.AddProject<Projects.MinimalWebAPI_API>("minimalwebapi-api")
    .WithReference(cosmosDb)
    .WithReference(serviceBus)
    .WithReference(cache);

builder.AddProject<Projects.MinimalWebAPI_UI>("minimalwebapi-ui")
    .WithReference(api);

builder.AddProject<Projects.MinimalWebAPI_Functions>("minimalwebapi-functions")
    .WithReference(serviceBus);

builder.Build().Run();
