// <copyright file="Program.cs" company="Newton Software">
// Copyright (c) Newton Software. All rights reserved.
// </copyright>

/*
 * This code is based on the following github repository: https://github.com/jonowilliams26/StructuredMinimalApi/tree/master
 * A video tutorial is available at: https://www.youtube.com/watch?v=ZA2X1gaAhJk&t=448s
 */

using FluentValidation;
using Microsoft.Azure.Cosmos;
using MinimalWebAPI;
using MinimalWebAPI.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.AddAzureCosmosClient(
    connectionName: "cosmos",
    configureClientOptions: options => options.SerializerOptions = new CosmosSerializationOptions { PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase });
builder.AddAzureServiceBusClient("messaging");
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