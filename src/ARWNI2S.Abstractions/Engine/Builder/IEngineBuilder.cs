namespace ARWNI2S.Engine.Builder
{
    /// <summary>
    /// Defines a class that provides the mechanisms to configure a engine's request pipeline.
    /// </summary>
    public interface IEngineBuilder
    {
        /// <summary>
        /// Gets or sets the <see cref="IServiceProvider"/> that provides access to the engine's service container.
        /// </summary>
        IServiceProvider EngineServices { get; set; }

        /// <summary>
        /// Gets the set of features the local node provides.
        /// </summary>
        /// <remarks>
        /// An empty collection is returned if a node wasn't specified for the engine builder.
        /// </remarks>
        IFeatureCollection NodeFeatures { get; }

        /// <summary>
        /// Gets a key/value collection that can be used to share data between processor.
        /// </summary>-
        IDictionary<string, object> Properties { get; }

        /// <summary>
        /// Registers a module for the simulation engine.
        /// </summary>
        /// <typeparam name="TModule">The type of the module to register.</typeparam>
        /// <param name="configuration">An optional configuration action for the module.</param>
        /// <returns>The <see cref="IEngineBuilder"/>.</returns>
        IEngineBuilder RegisterModule<TModule>(Action<TModule> configuration = null)
            where TModule : class, IEngineModule;

        /// <summary>
        /// Builds the delegate used by this engine to process HTTP requests.
        /// </summary>
        /// <returns>The request handling delegate.</returns>
        INiisEngine Build();

        /// <summary>
        /// Creates a new <see cref="IEngineBuilder"/> that shares the <see cref="Properties"/> of this
        /// <see cref="IEngineBuilder"/>.
        /// </summary>
        /// <returns>The new <see cref="IEngineBuilder"/>.</returns>
        IEngineBuilder New();
    }
}
