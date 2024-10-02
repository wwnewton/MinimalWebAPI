// <copyright file="CreateNoteTests.cs" company="Newton Software">
// Copyright (c) Newton Software. All rights reserved.
// </copyright>

namespace MinimalWebAPI.Tests.Features.Notes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Aspire.Hosting.Testing;
using FluentAssertions;
using MinimalWebAPI.Tests.Infrastructure;
using Snapshooter.Xunit;

/// <summary>
/// Create note tests.
/// </summary>
/// <param name="factory">App host factory.</param>
[Collection("SharedTestCollection")]
public class CreateNoteTests(AppHostFactory factory)
{
    private readonly AppHostFactory factory = factory;

    /// <summary>
    /// Create a valid Note.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task CreateValidNote()
    {
        // Arrange
        var apiClient = this.factory.App!.CreateHttpClient("minimalwebapi");

        // Act
        var response = await apiClient.PostAsJsonAsync("/notes", new { Name = "Test", Description = "Test Description" });

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        content.MatchSnapshot(matchOptions: matchOption => matchOption
            .Assert(fo => fo.Field<Guid>("id").Should().NotBeEmpty()));
    }
}
