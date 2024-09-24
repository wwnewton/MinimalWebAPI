// <copyright file="Program.cs" company="Newton Software">
// Copyright (c) Newton Software. All rights reserved.
// </copyright>

/*
 * This code is based on the following github repository: https://github.com/jonowilliams26/StructuredMinimalApi/tree/master
 * A video tutorial is available at: https://www.youtube.com/watch?v=ZA2X1gaAhJk&t=448s
 */

using FluentValidation;
using MinimalWebAPI;
using MinimalWebAPI.Features.TodoItems;
using MinimalWebAPI.Infrastructure.Persistence;
using MinimalWebAPI.Infrastructure.Validation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddSingleton(new Repository()); // A quick in memory repository for the demo to show how to inject services into endpoints

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapEndpoints();

await app.RunAsync();

/// <summary>
/// Program class.
/// Needed to be able to crate the WebApplication factory in unit tests.
/// </summary>
public partial class Program
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Program"/> class.
    /// </summary>
    protected Program()
    {
    }
}