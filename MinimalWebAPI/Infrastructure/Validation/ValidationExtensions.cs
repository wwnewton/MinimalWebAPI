// <copyright file="ValidationExtensions.cs" company="Newton Software">
// Copyright (c) Newton Software. All rights reserved.
// </copyright>

namespace MinimalWebAPI.Infrastructure.Validation;

/// <summary>
/// Adds validation extensions to RouteHandlerBuilder.
/// </summary>
public static class ValidationExtensions
{
    /// <summary>
    /// Adds request validation and validation problem response to RouteHandlerBuilder.
    /// </summary>
    /// <typeparam name="TRequest">Type of request.</typeparam>
    /// <param name="builder">Route handler builder.</param>
    /// <returns>The route handler builder.</returns>
    public static RouteHandlerBuilder WithRequestValidation<TRequest>(this RouteHandlerBuilder builder)
    {
        return builder.AddEndpointFilter<ValidationFilter<TRequest>>()
                      .ProducesValidationProblem();
    }
}
