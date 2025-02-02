﻿// <copyright file="CreateTodoItem.cs" company="Newton Software">
// Copyright (c) Newton Software. All rights reserved.
// </copyright>

namespace MinimalWebAPI.Features.TodoItems;

using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MinimalWebAPI.Infrastructure.Endpoints;
using MinimalWebAPI.Infrastructure.Persistence;
using MinimalWebAPI.Infrastructure.Validation;
using static MinimalWebAPI.Features.TodoItems.CreateTodoItem;

/// <summary>
/// Create a new TodoItem.
/// </summary>
public class CreateTodoItem : IEndpoint
{
    /// <inheritdoc/>
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapPost("/", Handle)
           .WithSummary("Create todo item.")
           .WithRequestValidation<CreateTodoItemRequest>();

    private static async Task<CreatedAtRoute<TodoItem>> Handle(CreateTodoItemRequest command, Repository repository)
    {
        var todoItem = new TodoItem(command.Title);
        await repository.AddAsync(todoItem);
        return TypedResults.CreatedAtRoute(todoItem, "GetTodoItemById", new { todoItem.Id });
    }

    /// <summary>
    /// Request.
    /// </summary>
    /// <param name="Title">TodoItem title.</param>
    public record CreateTodoItemRequest(string Title);

    /// <summary>
    /// Request validator.
    /// </summary>
    public class RequestValidator : AbstractValidator<CreateTodoItemRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestValidator"/> class.
        /// </summary>
        public RequestValidator()
        {
            this.RuleFor(x => x.Title).NotEmpty();
        }
    }
}
