using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Hosting.Builder;
using System;

namespace NI2S.Node.Hosting
{
    /// <summary>
    /// Used for initializing services and middlewares used by an application.
    /// </summary>
    public class DelegateStartup : StartupBase<IServiceCollection>
    {
        private readonly Action<INodeBuilder> _configureApp;

        /// <summary>
        /// Creates a new <see cref="DelegateStartup" /> instance.
        /// </summary>
        /// <param name="factory">A factory for creating <see cref="IServiceProvider"/> instances.</param>
        /// <param name="configureApp">An <see cref="Action"/> for configuring the application.</param>
        public DelegateStartup(IServiceProviderFactory<IServiceCollection> factory, Action<INodeBuilder> configureApp) : base(factory)
        {
            _configureApp = configureApp;
        }

        /// <summary>
        /// Configures the <see cref="INodeBuilder"/> with the initialized <see cref="Action"/>.
        /// </summary>
        /// <param name="app">The <see cref="INodeBuilder"/>.</param>
        public override void Configure(INodeBuilder app) => _configureApp(app);
    }
}
