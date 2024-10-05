// <copyright file="Program.cs" company="Newton Software">
// Copyright (c) Newton Software. All rights reserved.
// </copyright>

using MassTransit;
using Microsoft.Extensions.Hosting;
using MinimalWebAPI.Functions;

var builder = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .AddServiceDefaults();

builder.ConfigureServices(services =>
    services.AddMassTransitForAzureFunctions(
        x =>
        {
           x.AddConsumersFromNamespaceContaining<NoteConsumer>();
        },
        "ConnectionStrings:messaging"));

var host = builder.Build();

await host.RunAsync();
