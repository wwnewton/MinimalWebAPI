using Microsoft.Azure.Cosmos;
using MinimalWebAPI.CosmosInitializer;

var builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();
builder.AddAzureCosmosClient("cosmos", configureClientOptions: options => options.SerializerOptions = new CosmosSerializationOptions { PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase });
builder.Services.AddOpenTelemetry().WithTracing(tracerProviderBuilder =>
{
    tracerProviderBuilder.AddSource("Cosmos initializer");
});

var host = builder.Build();
await host.RunAsync();
