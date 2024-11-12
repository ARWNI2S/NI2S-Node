using ARWNI2S.Infrastructure.Engine.Builder;

namespace ARWNI2S.Runtime.Engine
{
    /// <summary>
    /// Defines a contract for a route builder in an application. A route builder specifies the routes for
    /// an application.
    /// </summary>
    public interface IMessageRelayBuilder
    {
        /// <summary>
        /// Creates a new <see cref="IEngineBuilder"/>.
        /// </summary>
        /// <returns>The new <see cref="IEngineBuilder"/>.</returns>
        IEngineBuilder CreateEngineBuilder();

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> used to resolve services for routes.
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Gets the endpoint data sources configured in the builder.
        /// </summary>
        ICollection<RelayDataSource> DataSources { get; }
    }
}