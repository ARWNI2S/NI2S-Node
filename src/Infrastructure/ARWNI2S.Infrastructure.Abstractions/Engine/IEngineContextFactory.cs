namespace ARWNI2S.Infrastructure.Engine
{
    /// <summary>
    /// Provides methods to create and dispose of <see cref="ExecutionContext"/> instances.
    /// </summary>
    public interface IEngineContextFactory
    {
        /// <summary>
        /// Creates an <see cref="ExecutionContext"/> instance for the specified set of HTTP features.
        /// </summary>
        /// <returns>The <see cref="ExecutionContext"/> instance.</returns>
        ExecutionContext Create(/*IFeatureCollection featureCollection*/);

        /// <summary>
        /// Releases resources held by the <see cref="ExecutionContext"/>.
        /// </summary>
        /// <param name="engineContext">The <see cref="ExecutionContext"/> to dispose.</param>
        void Dispose(ExecutionContext engineContext);
    }
}