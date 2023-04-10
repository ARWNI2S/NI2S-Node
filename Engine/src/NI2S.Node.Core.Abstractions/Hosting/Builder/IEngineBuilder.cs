using NI2S.Engine;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace NI2S.Node.Hosting.Builder
{
    /// <summary>
    /// Defines a class that provides the mechanisms to configure a node engine's message handling pipeline.
    /// </summary>
    public interface IEngineBuilder
    {
        /// <summary>
        /// Gets or sets the <see cref="IServiceProvider"/> that provides access to the engine's service container.
        /// </summary>
        IServiceProvider EngineServices { get; set; }

        /// <summary>
        /// Gets the set of modules the engine's node provides.
        /// </summary>
        /// <remarks>
        /// An empty collection is returned if a server wasn't specified for the node engine builder.
        /// </remarks>
        IModuleCollection EngineModules { get; }

        /// <summary>
        /// Gets a key/value collection that can be used to share data between middleware.
        /// </summary>
        IDictionary<string, object> Properties { get; }

        /// <summary>
        /// Creates a new <see cref="IEngineBuilder"/> that shares the <see cref="Properties"/> of this
        /// <see cref="IEngineBuilder"/>.
        /// </summary>
        /// <returns>The new <see cref="IEngineBuilder"/>.</returns>
        IEngineBuilder New();

        /// <summary>
        /// Builds the delegate used by this node to process engine messages.
        /// </summary>
        /// <returns>The message handling delegate.</returns>
        INodeEngine Build();
    }
}
