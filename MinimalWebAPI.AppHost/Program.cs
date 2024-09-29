var builder = DistributedApplication.CreateBuilder(args);

var cosmosDb = builder.AddAzureCosmosDB("cosmos")
    .RunAsEmulator().AddDatabase("todo");
builder.AddProject<Projects.MinimalWebAPI>("minimalwebapi")
    .WithReference(cosmosDb);

builder.Build().Run();
