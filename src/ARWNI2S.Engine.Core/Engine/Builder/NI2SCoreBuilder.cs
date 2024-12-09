using ARWNI2S.Core.Engine.Parts;
using ARWNI2S.Engine.Builder;
using ARWNI2S.Engine.Parts;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Core.Engine.Builder
{
    /// <summary>
    /// Allows fine grained configuration of essential NI2S services.
    /// </summary>
    internal sealed class NI2SCoreBuilder : INiisCoreBuilder
    {
        /// <summary>
        /// Initializes a new <see cref="NI2SCoreBuilder"/> instance.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="manager">The <see cref="ApplicationPartManager"/> of the application.</param>
        public NI2SCoreBuilder(
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

        IApplicationPartManager INiisCoreBuilder.PartManager => PartManager;
    }
}
