// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using NI2S.Node.Data;
using System;
using System.Collections.Generic;

namespace NI2S.Node.Hosting.Builder
{
    //TODO: BUILD CLUSTERING AROUND THAT
    public interface IClusterNodeBuilder
    {
        /// <summary>
        /// Creates a new <see cref="IEngineBuilder"/>.
        /// </summary>
        /// <returns>The new <see cref="IEngineBuilder"/>.</returns>
        IEngineBuilder CreateEngineBuilder();

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> used to resolve services for routes.
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Gets the external data sources configured in the builder.
        /// </summary>
        ICollection<IDataSource> DataSources { get; }
    }
}
