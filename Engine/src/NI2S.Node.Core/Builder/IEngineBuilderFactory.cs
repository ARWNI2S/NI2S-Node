using Microsoft.AspNetCore.Http.Features;

namespace NI2S.Node.Builder
{
    /// <summary>
    /// Provides an interface for implementing a factory that produces <see cref="IEngineBuilder"/> instances.
    /// </summary>
    public interface IEngineBuilderFactory
    {
        /// <summary>
        /// Create an <see cref="IEngineBuilder" /> builder given a <paramref name="serverFeatures" />
        /// </summary>
        /// <param name="serverFeatures">An <see cref="IFeatureCollection"/> of HTTP features.</param>
        /// <returns>An <see cref="IEngineBuilder"/> configured with <paramref name="serverFeatures"/>.</returns>
        IEngineBuilder CreateBuilder(IFeatureCollection serverFeatures);
    }
}
