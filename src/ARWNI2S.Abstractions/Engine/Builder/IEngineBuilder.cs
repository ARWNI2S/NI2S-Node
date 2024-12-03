using ARWNI2S.Extensibility;

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
        IModuleCollection NodeModules { get; }

        /// <summary>
        /// Gets a key/value collection that can be used to share data between middleware.
        /// </summary>
        IDictionary<string, object> Properties { get; }

        ///// <summary>
        ///// Adds a update processor delegate to the engine's update pipeline.
        ///// </summary>
        ///// <param name="processor">The update processor delegate.</param>
        ///// <returns>The <see cref="IEngineBuilder"/>.</returns>
        //IEngineBuilder Use(Func<UpdateDelegate, UpdateDelegate> processor);

        ///// <summary>
        ///// Builds the delegate used by this engine to process HTTP requests.
        ///// </summary>
        ///// <returns>The request handling delegate.</returns>
        //UpdateDelegate Build();

        ///// <summary>
        ///// Creates a new <see cref="IEngineBuilder"/> that shares the <see cref="Properties"/> of this
        ///// <see cref="IEngineBuilder"/>.
        ///// </summary>
        ///// <returns>The new <see cref="IEngineBuilder"/>.</returns>
        //IEngineBuilder New();
    }
}
