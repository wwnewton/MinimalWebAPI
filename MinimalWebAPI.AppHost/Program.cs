var builder = DistributedApplication.CreateBuilder(args);

var cosmosDb = builder.AddAzureCosmosDB("cosmos")
    .RunAsEmulator().AddDatabase("todo");

var api = builder.AddProject<Projects.MinimalWebAPI>("minimalwebapi")
    .WithReference(cosmosDb);

builder.AddProject<Projects.MinimalWebAPI_UI>("minimalwebapi-ui")
    .WithReference(api);

builder.Build().Run();
