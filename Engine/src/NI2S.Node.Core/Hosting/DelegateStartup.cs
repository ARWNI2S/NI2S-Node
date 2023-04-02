using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Builder;
using System;

namespace NI2S.Node.Hosting
{
    /// <summary>
    /// Used for initializing services and middlewares used by an application.
    /// </summary>
    public class DelegateStartup : StartupBase<IServiceCollection>
    {
        private readonly Action<IEngineBuilder> _configureApp;

        /// <summary>
        /// Creates a new <see cref="DelegateStartup" /> instance.
        /// </summary>
        /// <param name="factory">A factory for creating <see cref="IServiceProvider"/> instances.</param>
        /// <param name="configureApp">An <see cref="Action"/> for configuring the application.</param>
        public DelegateStartup(IServiceProviderFactory<IServiceCollection> factory, Action<IEngineBuilder> configureApp) : base(factory)
        {
            _configureApp = configureApp;
        }

        /// <summary>
        /// Configures the <see cref="IEngineBuilder"/> with the initialized <see cref="Action"/>.
        /// </summary>
        /// <param name="app">The <see cref="IEngineBuilder"/>.</param>
        public override void Configure(IEngineBuilder app) => _configureApp(app);
    }
}
