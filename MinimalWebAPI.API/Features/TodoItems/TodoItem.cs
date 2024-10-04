// <copyright file="TodoItem.cs" company="Newton Software">
// Copyright (c) Newton Software. All rights reserved.
// </copyright>

namespace MinimalWebAPI.API.Features.TodoItems;

using MinimalWebAPI.API.Infrastructure.Persistence;

/// <summary>
/// TodoItem.
/// </summary>
public class TodoItem : IEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TodoItem"/> class.
    /// </summary>
    /// <param name="title">Title of item.</param>
    public TodoItem(string title)
    {
        this.Title = title;
        this.CreatedDate = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Gets a unique identifier for the item.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Gets the title of the item.
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// Gets the date the item was created.
    /// </summary>
    public DateTimeOffset CreatedDate { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the item is complete.
    /// </summary>
    public bool IsComplete { get; private set; }

    /// <summary>
    /// Gets the date the item was completed.
    /// </summary>
    public DateTimeOffset? CompletedDate { get; private set; }

    /// <summary>
    /// Marks the item as complete.
    /// </summary>
    public void MarkComplete()
    {
        this.IsComplete = true;
        this.CompletedDate = DateTimeOffset.UtcNow;
    }
}
