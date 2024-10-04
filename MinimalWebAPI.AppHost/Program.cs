var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var cosmosDb = builder.AddConnectionString("cosmos");

// This initializes the Cosmos DB database and containers for cosmos emulator.
var cosmosInitializer = builder.AddProject<Projects.MinimalWebAPI_CosmosInitializer>("minimalwebapi-cosmosinitializer")
    .WithReference(cosmosDb);

var serviceBus = builder.AddConnectionString("messaging");

var api = builder.AddProject<Projects.MinimalWebAPI_API>("minimalwebapi-api")
    .WithReference(cosmosDb)
    .WithReference(serviceBus)
    .WithReference(cache);

builder.AddProject<Projects.MinimalWebAPI_UI>("minimalwebapi-ui")
    .WithReference(api);



builder.AddProject<Projects.MinimalWebAPI_Functions>("minimalwebapi-functions")
    .WithReference(serviceBus);



builder.Build().Run();
