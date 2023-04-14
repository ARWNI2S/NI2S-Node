// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using System;

namespace NI2S.Node.Hosting.Builder
{
    //TODO: BUILD CLUSTERING AROUND THAT
    public interface IClusterNodeBuilder
    {
        /// <summary>
        /// Creates a new <see cref="INodeEngineBuilder"/>.
        /// </summary>
        /// <returns>The new <see cref="INodeEngineBuilder"/>.</returns>
        INodeEngineBuilder CreateEngineBuilder();

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> used to resolve services for routes.
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Gets the endpoint data sources configured in the builder.
        /// </summary>
        //ICollection<EndpointDataSource> DataSources { get; }
    }

}
