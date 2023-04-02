using Microsoft.AspNetCore.Http.Features;
using System;

namespace NI2S.Node.Builder
{
    /// <summary>
    /// A factory for creating <see cref="IEngineBuilder" /> instances.
    /// </summary>
    public class EngineBuilderFactory : IEngineBuilderFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initialize a new factory instance with an <see cref="IServiceProvider" />.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> used to resolve dependencies and initialize components.</param>
        public EngineBuilderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Create an <see cref="IEngineBuilder" /> builder given a <paramref name="serverFeatures" />.
        /// </summary>
        /// <param name="serverFeatures">An <see cref="IFeatureCollection"/> of HTTP features.</param>
        /// <returns>An <see cref="IEngineBuilder"/> configured with <paramref name="serverFeatures"/>.</returns>
        public IEngineBuilder CreateBuilder(IFeatureCollection serverFeatures)
        {
            return new EngineBuilder(_serviceProvider, serverFeatures);
        }
    }
}
