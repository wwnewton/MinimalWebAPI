// <copyright file="CreateTodoItemTests.cs" company="Newton Software">
// Copyright (c) Newton Software. All rights reserved.
// </copyright>

namespace MinimalWebAPI.Tests.Features.TodoItems;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using MinimalWebAPI.Features.TodoItems;
using Xunit;

/// <summary>
/// TodoItem tests.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CreateTodoItemTests"/> class.
/// </remarks>
/// <param name="factory">Web application factory.</param>
public class CreateTodoItemTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> factory = factory;

    /// <summary>
    /// Create a valid TodoItem.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task CreateValidTodoItem()
    {
        // Arrange
        var client = this.factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/todo-items", new { Title = "Test" });

        // Assert
        response.EnsureSuccessStatusCode();
        var todoItem = await response.Content.ReadFromJsonAsync<TodoItem>();
        Assert.NotNull(todoItem);
        Assert.Equal("Test", todoItem.Title);
    }
}
