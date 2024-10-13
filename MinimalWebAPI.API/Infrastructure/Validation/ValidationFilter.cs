// <copyright file="ValidationFilter.cs" company="Newton Software">
// Copyright (c) Newton Software. All rights reserved.
// </copyright>

namespace MinimalWebAPI.API.Infrastructure.Validation;

using FluentValidation;

/// <summary>
/// Validation filter.
/// </summary>
/// <typeparam name="TRequest">Type of request.</typeparam>
public class ValidationFilter<TRequest> : IEndpointFilter
{
    private readonly IValidator<TRequest> validator;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationFilter{TRequest}"/> class.
    /// </summary>
    /// <param name="validator">Validator for request type.</param>
    public ValidationFilter(IValidator<TRequest> validator)
    {
        this.validator = validator;
    }

    /// <inheritdoc/>
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        ArgumentNullException.ThrowIfNull(context);
        var request = context.Arguments.OfType<TRequest>().First();
        var result = await this.validator.ValidateAsync(request, context.HttpContext.RequestAborted);
        if (!result.IsValid)
        {
            return TypedResults.ValidationProblem(result.ToDictionary());
        }

        return next is null ? default : await next(context);
    }
}
