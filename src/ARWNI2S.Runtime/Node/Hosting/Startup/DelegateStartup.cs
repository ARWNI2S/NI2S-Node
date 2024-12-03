using ARWNI2S.Engine.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Node.Hosting.Startup
{
    /// <summary>
    /// Used for initializing services and middlewares used by a engine.
    /// </summary>
    public class DelegateStartup : StartupBase<IServiceCollection>
    {
        private readonly Action<IEngineBuilder> _configureEngine;

        /// <summary>
        /// Creates a new <see cref="DelegateStartup" /> instance.
        /// </summary>
        /// <param name="factory">A factory for creating <see cref="IServiceProvider"/> instances.</param>
        /// <param name="configureEngine">An <see cref="Action"/> for configuring the engine.</param>
        public DelegateStartup(IServiceProviderFactory<IServiceCollection> factory, Action<IEngineBuilder> configureEngine) : base(factory)
        {
            _configureEngine = configureEngine;
        }

        /// <summary>
        /// Configures the <see cref="IEngineBuilder"/> with the initialized <see cref="Action"/>.
        /// </summary>
        /// <param name="engine">The <see cref="IEngineBuilder"/>.</param>
        public override void Configure(IEngineBuilder engine) => _configureEngine(engine);
    }
}