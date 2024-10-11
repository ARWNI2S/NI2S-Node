using ARWNI2S.Node.Features;

namespace ARWNI2S.Node.Builder
{
    /// <summary>
    /// Defines a class that provides the mechanisms to configure an application's request pipeline.
    /// </summary>
    public interface INodeBuilder
    {
        /// <summary>
        /// Gets or sets the <see cref="IServiceProvider"/> that provides access to the application's service container.
        /// </summary>
        IServiceProvider ApplicationServices { get; set; }

        /// <summary>
        /// Gets the set of HTTP features the application's server provides.
        /// </summary>
        /// <remarks>
        /// An empty collection is returned if a server wasn't specified for the application builder.
        /// </remarks>
        IFeatureCollection ServerFeatures { get; }

        /// <summary>
        /// Gets a key/value collection that can be used to share data between middleware.
        /// </summary>
        IDictionary<string, object?> Properties { get; }

        /// <summary>
        /// Adds a middleware delegate to the application's request pipeline.
        /// </summary>
        /// <param name="middleware">The middleware delegate.</param>
        /// <returns>The <see cref="INodeBuilder"/>.</returns>
        INodeBuilder Use(Func<RequestDelegate, RequestDelegate> middleware);

        /// <summary>
        /// Creates a new <see cref="INodeBuilder"/> that shares the <see cref="Properties"/> of this
        /// <see cref="INodeBuilder"/>.
        /// </summary>
        /// <returns>The new <see cref="INodeBuilder"/>.</returns>
        INodeBuilder New();

        /// <summary>
        /// Builds the delegate used by this application to process HTTP requests.
        /// </summary>
        /// <returns>The request handling delegate.</returns>
        RequestDelegate Build();
    }
}
