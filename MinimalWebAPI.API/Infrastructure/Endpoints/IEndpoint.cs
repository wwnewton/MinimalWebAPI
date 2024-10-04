// <copyright file="IEndpoint.cs" company="Newton Software">
// Copyright (c) Newton Software. All rights reserved.
// </copyright>

namespace MinimalWebAPI.API.Infrastructure.Endpoints;

/// <summary>
/// Interface for an endpoint.
/// </summary>
public interface IEndpoint
{
    /// <summary>
    /// Map endpoint.
    /// </summary>
    /// <param name="app">Endpoint route builder.</param>
    public static abstract void Map(IEndpointRouteBuilder app);
}
