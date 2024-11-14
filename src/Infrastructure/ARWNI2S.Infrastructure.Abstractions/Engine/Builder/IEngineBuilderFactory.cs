namespace ARWNI2S.Infrastructure.Engine.Builder
{
    /// <summary>
    /// Provides an interface for implementing a factory that produces <see cref="IEngineBuilder"/> instances.
    /// </summary>
    public interface IEngineBuilderFactory
    {
        /// <summary>
        /// Create an <see cref="IEngineBuilder" /> builder given a <paramref name="engineFeatures" />
        /// </summary>
        /// <param name="engineFeatures">An <see cref="IFeatureCollection"/> of NI2S features.</param>
        /// <returns>An <see cref="IEngineBuilder"/> configured with <paramref name="engineFeatures"/>.</returns>
        IEngineBuilder CreateBuilder(IFeatureCollection engineFeatures);
    }
}