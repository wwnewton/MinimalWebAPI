// <copyright file="SharedTestSetup.cs" company="Newton Software">
// Copyright (c) Newton Software. All rights reserved.
// </copyright>

namespace MinimalWebAPI.Tests.Infrastructure;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Shared test collection.
/// </summary>
[CollectionDefinition("SharedTestCollection")]
public class SharedTestSetup : ICollectionFixture<AppHostFactory>
{
}
