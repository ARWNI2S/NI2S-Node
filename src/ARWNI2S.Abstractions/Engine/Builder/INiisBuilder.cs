using ARWNI2S.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Engine.Builder
{
    public interface INiisBuilder
    {
        IServiceCollection Services { get; }

        ApplicationPartManager PartManager { get; }
        //EnginePartManager PartManager { get; }
    }

    /// <summary>
    /// An interface for configuring essential NI2S services.
    /// </summary>
    public interface INiisCoreBuilder
    {
        /// <summary>
        /// Gets the <see cref="IServiceCollection"/> where essential NI2S services are configured.
        /// </summary>
        IServiceCollection Services { get; }

        /// <summary>
        /// Gets the <see cref="ApplicationPartManager"/> where <see cref="ApplicationPart"/>s
        /// are configured.
        /// </summary>
        ApplicationPartManager PartManager { get; }
    }
}