// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Engine.Parts;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Engine.Core.Builder
{
    internal class NI2SCoreBuilder : INiisCoreBuilder
    {
        /// <summary>
        /// Initializes a new <see cref="NI2SCoreBuilder"/> instance.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="manager">The <see cref="EnginePartManager"/> of the application.</param>
        public NI2SCoreBuilder(
            IServiceCollection services,
            EnginePartManager manager)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(manager);

            Services = services;
            PartManager = manager;
        }

        /// <inheritdoc />
        public EnginePartManager PartManager { get; }

        /// <inheritdoc />
        public IServiceCollection Services { get; }

        IEnginePartManager INiisCoreBuilder.PartManager => PartManager;
    }
}