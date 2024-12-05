using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Engine.Builder
{
    /// <summary>
    /// Allows fine grained configuration of essential MVRM services.
    /// </summary>
    internal sealed class NiisBuilder : INiisBuilder
    {
        /// <summary>
        /// Initializes a new <see cref="NiisBuilder"/> instance.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="manager">The <see cref="IEnginePartManager"/> of the engine.</param>
        public NiisBuilder(
            IServiceCollection services,
            IEnginePartManager manager)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(manager);

            Services = services;
            PartManager = manager;
        }

        /// <inheritdoc />
        public IEnginePartManager PartManager { get; }

        /// <inheritdoc />
        public IServiceCollection Services { get; }
    }
}