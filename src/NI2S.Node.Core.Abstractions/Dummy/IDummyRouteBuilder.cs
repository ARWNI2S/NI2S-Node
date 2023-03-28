using NI2S.Node.Hosting.Builder;
using System;
using System.Collections.Generic;

namespace NI2S.Node.Dummy
{
    /// <summary>
    /// Defines a contract for a route builder in an node. A route builder specifies the routes for
    /// an node.
    /// </summary>
    public interface IDummyRouteBuilder
    {
        /// <summary>
        /// Gets the <see cref="INodeBuilder"/>.
        /// </summary>
        INodeBuilder NodeBuilder { get; }

        /// <summary>
        /// Gets or sets the default <see cref="IDummyRouter"/> that is used as a handler if an <see cref="IDummyRouter"/>
        /// is added to the list of routes but does not specify its own.
        /// </summary>
        IDummyRouter DefaultHandler { get; set; }

        /// <summary>
        /// Gets the sets the <see cref="IServiceProvider"/> used to resolve services for routes.
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Gets the routes configured in the builder.
        /// </summary>
        IList<IDummyRouter> Routes { get; }

        /// <summary>
        /// Builds an <see cref="IDummyRouter"/> that routes the routes specified in the <see cref="Routes"/> property.
        /// </summary>
        IDummyRouter Build();
    }
}
