using ARWNI2S.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Engine.Builder
{
    /// <summary>
    /// Allows fine grained configuration of NI2S services.
    /// </summary>
    internal sealed class NiisBuilder : INiisBuilder
    {
        /// <summary>
        /// Initializes a new <see cref="NiisBuilder"/> instance.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="manager">The <see cref="ApplicationPartManager"/> of the application.</param>
        public NiisBuilder(IServiceCollection services, ApplicationPartManager manager)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(manager);

            Services = services;
            PartManager = manager;
        }

        /// <inheritdoc />
        public IServiceCollection Services { get; }

        /// <inheritdoc />
        public ApplicationPartManager PartManager { get; }
    }

    /// <summary>
    /// Allows fine grained configuration of essential NI2S services.
    /// </summary>
    internal sealed class NiisCoreBuilder : INiisCoreBuilder
    {
        /// <summary>
        /// Initializes a new <see cref="NiisCoreBuilder"/> instance.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="manager">The <see cref="ApplicationPartManager"/> of the application.</param>
        public NiisCoreBuilder(
            IServiceCollection services,
            ApplicationPartManager manager)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(manager);

            Services = services;
            PartManager = manager;
        }

        /// <inheritdoc />
        public ApplicationPartManager PartManager { get; }

        /// <inheritdoc />
        public IServiceCollection Services { get; }
    }
}