using NI2S.Node.Engine.Modules;
using System;

namespace NI2S.Node.Hosting.Builder
{
    /// <summary>
    /// A factory for creating <see cref="INodeBuilder" /> instances.
    /// </summary>
    public class NodeBuilderFactory : INodeBuilderFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initialize a new factory instance with an <see cref="IServiceProvider" />.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> used to resolve dependencies and initialize components.</param>
        public NodeBuilderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Create an <see cref="INodeBuilder" /> builder given a <paramref name="serverFeatures" />.
        /// </summary>
        /// <param name="serverFeatures">An <see cref="IDummyFeatureCollection"/> of HTTP features.</param>
        /// <returns>An <see cref="INodeBuilder"/> configured with <paramref name="serverFeatures"/>.</returns>
        public INodeBuilder CreateBuilder(IDummyFeatureCollection serverFeatures)
        {
            return new NodeBuilder(_serviceProvider, serverFeatures);
        }
    }
}
