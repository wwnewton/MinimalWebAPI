var builder = DistributedApplication.CreateBuilder(args);

var cosmosDb = builder.AddAzureCosmosDB("cosmos")
    .RunAsEmulator().AddDatabase("todo");

// This initializes the Cosmos DB database and containers for cosmos emulator.
var cosmosInitializer = builder.AddProject<Projects.MinimalWebAPI_CosmosInitializer>("minimalwebapi-cosmosinitializer")
    .WithReference(cosmosDb);

var api = builder.AddProject<Projects.MinimalWebAPI>("minimalwebapi")
    .WithReference(cosmosDb);

builder.AddProject<Projects.MinimalWebAPI_UI>("minimalwebapi-ui")
    .WithReference(api);



builder.Build().Run();
