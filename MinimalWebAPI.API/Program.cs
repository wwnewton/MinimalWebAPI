// <copyright file="Program.cs" company="Newton Software">
// Copyright (c) Newton Software. All rights reserved.
// </copyright>

/*
 * This code is based on the following github repository: https://github.com/jonowilliams26/StructuredMinimalApi/tree/master
 * A video tutorial is available at: https://www.youtube.com/watch?v=ZA2X1gaAhJk&t=448s
 */

using FluentValidation;
using MassTransit;
using MassTransit.Middleware;
using Microsoft.Azure.Cosmos;
using MinimalWebAPI;
using MinimalWebAPI.API.Features.Notes;
using MinimalWebAPI.API.Infrastructure.Persistence;
using MinimalWebAPI.Shared.Notes;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.AddAzureCosmosClient(
    connectionName: "cosmos",
    configureClientOptions: options => options.SerializerOptions = new CosmosSerializationOptions { PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase });
builder.Services.AddMassTransit(x =>
{
    x.UsingAzureServiceBus((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("messaging"));
        EndpointConvention.Map<NoteCreated>(new Uri("queue:test"));
        GlobalTopology.Send.UseCorrelationId<NoteCreated>(x => x.Id);
    });
});
builder.AddRedisDistributedCache("cache");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddSingleton<Repository>();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapEndpoints();

await app.RunAsync();