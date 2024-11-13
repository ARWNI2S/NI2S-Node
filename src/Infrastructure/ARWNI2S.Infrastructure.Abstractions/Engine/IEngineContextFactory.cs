namespace ARWNI2S.Infrastructure.Engine
{
    /// <summary>
    /// Provides methods to create and dispose of <see cref="EngineContext"/> instances.
    /// </summary>
    public interface IEngineContextFactory
    {
        /// <summary>
        /// Creates an <see cref="EngineContext"/> instance for the specified set of HTTP features.
        /// </summary>
        /// <returns>The <see cref="EngineContext"/> instance.</returns>
        EngineContext Create(IFeatureCollection featureCollection);

        /// <summary>
        /// Releases resources held by the <see cref="EngineContext"/>.
        /// </summary>
        /// <param name="engineContext">The <see cref="EngineContext"/> to dispose.</param>
        void Dispose(EngineContext engineContext);
    }
}