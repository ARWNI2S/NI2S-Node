using Microsoft.Extensions.Hosting;

namespace ARWNI2S.Infrastructure.Hosting
{
    public interface INI2SBuilder
    {
        /// <summary>
        /// Gets or sets the <see cref="IServiceProvider"/> that provides access to the application's service container.
        /// </summary>
        IServiceProvider ApplicationServices { get; set; }

        ///// <summary>
        ///// Gets the set of HTTP features the application's server provides.
        ///// </summary>
        ///// <remarks>
        ///// An empty collection is returned if a server wasn't specified for the application builder.
        ///// </remarks>
        //IFeatureCollection ServerFeatures { get; }

        /// <summary>
        /// Gets a key/value collection that can be used to share data between middleware.
        /// </summary>
        IDictionary<string, object> Properties { get; }

        ///// <summary>
        ///// Adds a middleware delegate to the application's request pipeline.
        ///// </summary>
        ///// <param name="middleware">The middleware delegate.</param>
        ///// <returns>The <see cref="INI2SBuilder"/>.</returns>
        //INI2SBuilder Use(Func<RequestDelegate, RequestDelegate> middleware);

        /// <summary>
        /// Creates a new <see cref="INI2SBuilder"/> that shares the <see cref="Properties"/> of this
        /// <see cref="INI2SBuilder"/>.
        /// </summary>
        /// <returns>The new <see cref="INI2SBuilder"/>.</returns>
        INI2SBuilder New();

        ///// <summary>
        ///// Builds the delegate used by this application to process HTTP requests.
        ///// </summary>
        ///// <returns>The request handling delegate.</returns>
        //RequestDelegate Build();
        IHost Build();
    }
}