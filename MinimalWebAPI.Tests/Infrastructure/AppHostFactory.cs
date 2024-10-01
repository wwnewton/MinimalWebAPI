// <copyright file="AppHostFactory.cs" company="Newton Software">
// Copyright (c) Newton Software. All rights reserved.
// </copyright>

namespace MinimalWebAPI.Tests.Infrastructure;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspire.Hosting;
using Aspire.Hosting.Testing;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// App host factory.
/// </summary>
public class AppHostFactory : IAsyncLifetime
{
    /// <summary>
    /// Gets the distributed application.
    /// </summary>
    public DistributedApplication? App { get; private set; }

    /// <summary>
    /// Gets the resource notification service.
    /// </summary>
    public ResourceNotificationService? ResourceNotificationService { get; private set; }

    /// <inheritdoc/>
    public async Task DisposeAsync()
    {
        await this.App!.DisposeAsync();
    }

    /// <inheritdoc/>
    public async Task InitializeAsync()
    {
        var appHost = await DistributedApplicationTestingBuilder
            .CreateAsync<Projects.MinimalWebAPI_AppHost>();
        this.App = await appHost.BuildAsync();
        this.ResourceNotificationService = this.App.Services.GetRequiredService<ResourceNotificationService>();
        await this.App.StartAsync();
        await this.ResourceNotificationService.WaitForResourceAsync("minimalwebapi-cosmosinitializer", KnownResourceStates.Finished).WaitAsync(TimeSpan.FromMinutes(3));
    }
}
