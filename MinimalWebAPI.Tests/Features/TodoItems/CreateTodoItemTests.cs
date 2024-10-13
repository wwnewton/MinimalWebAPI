// <copyright file="CreateTodoItemTests.cs" company="Newton Software">
// Copyright (c) Newton Software. All rights reserved.
// </copyright>

namespace MinimalWebAPI.Tests.Features.TodoItems;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Aspire.Hosting.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using MinimalWebAPI.Tests.Infrastructure;
using Snapshooter.Xunit;
using Xunit;

/// <summary>
/// TodoItem tests.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CreateTodoItemTests"/> class.
/// </remarks>
[Collection("SharedTestCollection")]
public class CreateTodoItemTests(AppHostFactory factory)
{
    private readonly AppHostFactory factory = factory;

    /// <summary>
    /// Create a valid TodoItem.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task CreateValidTodoItem()
    {
        // Arrange
        using var apiClient = this.factory.App!.CreateHttpClient("minimalwebapi-api");

        // Act
        var response = await apiClient.PostAsJsonAsync("/todo-items", new { Title = "Test" });

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        content.MatchSnapshot(matchOptions: matchOption => matchOption
            .Assert(fo => fo.Field<Guid>("id").Should().NotBeEmpty())
            .Assert(fo => fo.Field<DateTime?>("createdDate").Should().NotBeNull()));
    }
}
