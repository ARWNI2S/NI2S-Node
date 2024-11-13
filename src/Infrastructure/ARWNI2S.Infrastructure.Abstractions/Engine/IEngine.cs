namespace ARWNI2S.Infrastructure.Engine
{
    /// <summary>
    /// Represents a ni2s engine.
    /// </summary>
    public interface IEngine
    {
        IFeatureCollection Features { get; }

        Task StartAsync<TContext>(IEngine<TContext> engine, CancellationToken cancellationToken) where TContext : notnull;

        Task StopAsync(CancellationToken cancellationToken);
    }

    /// <summary>
    /// Represents a ni2s engine.
    /// </summary>
    /// <typeparam name="TContext">The context associated with the engine.</typeparam>
    public interface IEngine<TContext> where TContext : notnull
    {
        /// <summary>
        /// Create a TContext given a collection of ni2s features.
        /// </summary>
        /// <param name="contextFeatures">A collection of ni2s features to be used for creating the TContext.</param>
        /// <returns>The created TContext.</returns>
        TContext CreateContext(IFeatureCollection contextFeatures);

        /// <summary>
        /// Asynchronously processes a TContext.
        /// </summary>
        /// <param name="context">The TContext that the operation will process.</param>
        Task ProcessFrameAsync(TContext context);

        /// <summary>
        /// Dispose a given TContext.
        /// </summary>
        /// <param name="context">The TContext to be disposed.</param>
        /// <param name="exception">The Exception thrown when processing did not complete successfully, otherwise null.</param>
        void DisposeContext(TContext context, Exception exception);
    }

}
