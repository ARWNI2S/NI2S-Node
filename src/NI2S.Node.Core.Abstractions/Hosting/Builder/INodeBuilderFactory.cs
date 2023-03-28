using NI2S.Node.Engine.Modules;

namespace NI2S.Node.Hosting.Builder
{
    /// <summary>
    /// Provides an interface for implementing a factory that produces <see cref="INodeBuilder"/> instances.
    /// </summary>
    public interface INodeBuilderFactory
    {
        /// <summary>
        /// Create an <see cref="INodeBuilder" /> builder given a <paramref name="serverFeatures" />
        /// </summary>
        /// <param name="serverFeatures">An <see cref="IDummyFeatureCollection"/> of HTTP features.</param>
        /// <returns>An <see cref="INodeBuilder"/> configured with <paramref name="serverFeatures"/>.</returns>
        INodeBuilder CreateBuilder(IDummyFeatureCollection serverFeatures);
    }
}
