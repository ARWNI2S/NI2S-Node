﻿using NI2S.Node.Hosting;
using NI2S.Node.Hosting.Builder;
using System;
using System.Collections.Generic;

namespace NI2S.Node.Routing
{
    /// <summary>
    /// Defines a contract for a route builder in an application. A route builder specifies the routes for
    /// an application.
    /// </summary>
    public interface IEndpointRouteBuilder
    {
        /// <summary>
        /// Creates a new <see cref="IApplicationBuilder"/>.
        /// </summary>
        /// <returns>The new <see cref="IApplicationBuilder"/>.</returns>
        IApplicationBuilder CreateApplicationBuilder();

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> used to resolve services for routes.
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Gets the endpoint data sources configured in the builder.
        /// </summary>
        ICollection<EndpointDataSource> DataSources { get; }
    }
}
