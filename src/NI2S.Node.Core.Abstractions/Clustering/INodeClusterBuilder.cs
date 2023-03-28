using NI2S.Node.Dummy;
using NI2S.Node.Hosting.Builder;
using System;
using System.Collections.Generic;

namespace NI2S.Node.Clustering
{
    /// <summary>
    /// Defines a contract for a route builder in an node. A route builder specifies the routes for
    /// an node.
    /// </summary>
    public interface INodeClusterBuilder
    {
        /// <summary>
        /// Creates a new <see cref="INodeBuilder"/>.
        /// </summary>
        /// <returns>The new <see cref="INodeBuilder"/>.</returns>
        INodeBuilder CreateNodeBuilder();

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> used to resolve services for routes.
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Gets the endpoint data sources configured in the builder.
        /// </summary>
        ICollection<DummyDataSource> DataSources { get; }
    }
}
