using NI2S.Node.Dummy;
using NI2S.Node.Engine.Modules;
using System;
using System.Collections.Generic;

namespace NI2S.Node.Hosting.Builder
{
    /// <summary>
    /// Defines a class that provides the mechanisms to configure a NI2S engine message handling pipeline.
    /// </summary>
    public interface INodeBuilder
    {
        /// <summary>
        /// Gets or sets the <see cref="IServiceProvider"/> that provides access to the node's service container.
        /// </summary>
        IServiceProvider ApplicationServices { get; set; }

        /// <summary>
        /// Gets the set of HTTP features the node's server provides.
        /// </summary>
        /// <remarks>
        /// An empty collection is returned if a server wasn't specified for the node builder.
        /// </remarks>
        IDummyFeatureCollection ServerFeatures { get; }

        /// <summary>
        /// Gets a key/value collection that can be used to share data between middleware.
        /// </summary>
        IDictionary<string, object> Properties { get; }

        /// <summary>
        /// Adds a middleware delegate to the node's request pipeline.
        /// </summary>
        /// <param name="middleware">The middleware delegate.</param>
        /// <returns>The <see cref="INodeBuilder"/>.</returns>
        INodeBuilder Use(Func<MessageDelegate, MessageDelegate> middleware);

        /// <summary>
        /// Creates a new <see cref="INodeBuilder"/> that shares the <see cref="Properties"/> of this
        /// <see cref="INodeBuilder"/>.
        /// </summary>
        /// <returns>The new <see cref="INodeBuilder"/>.</returns>
        INodeBuilder New();

        /// <summary>
        /// Builds the delegate used by this node to process HTTP requests.
        /// </summary>
        /// <returns>The request handling delegate.</returns>
        MessageDelegate Build();
    }
}
