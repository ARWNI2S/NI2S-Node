using ARWNI2S.Engine.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Node.Hosting.Startup
{
    /// <summary>
    /// Base class for initializing services and middlewares used by a engine.
    /// </summary>
    public abstract class StartupBase : INodeStartup
    {
        /// <summary>
        /// Configures the engine.
        /// </summary>
        /// <param name="engine">An <see cref="IEngineBuilder"/> for the engine to configure.</param>
        public abstract void Configure(IEngineBuilder engine);

        IServiceProvider INodeStartup.ConfigureServices(IServiceCollection services)
        {
            ConfigureServices(services);
            return CreateServiceProvider(services);
        }

        /// <summary>
        /// Register services into the <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        public virtual void ConfigureServices(IServiceCollection services)
        {
        }

        /// <summary>
        /// Creates an <see cref="IServiceProvider" /> instance for a given <see cref="ConfigureServices(IServiceCollection)" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns>The <see cref="IServiceProvider"/>.</returns>
        public virtual IServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            return services.BuildServiceProvider();
        }
    }

    /// <summary>
    /// Base class for initializing services and middlewares used for configuring a <typeparamref name="TBuilder"/>.
    /// </summary>
    /// <typeparam name="TBuilder">The type of builder associated with the startup configuration.</typeparam>
    public abstract class StartupBase<TBuilder> : StartupBase where TBuilder : notnull
    {
        private readonly IServiceProviderFactory<TBuilder> _factory;

        /// <summary>
        /// Constructor for StartupBase class.
        /// </summary>
        /// <param name="factory">A factory used to generate <see cref="IServiceProvider"/> instances.</param>
        public StartupBase(IServiceProviderFactory<TBuilder> factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Creates an <see cref="IServiceProvider" /> instance for a given <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns>The <see cref="IServiceProvider"/>.</returns>
        public override IServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            var builder = _factory.CreateBuilder(services);
            ConfigureContainer(builder);
            return _factory.CreateServiceProvider(builder);
        }

        /// <summary>
        /// Sets up the service container.
        /// </summary>
        /// <param name="builder">The builder associated with the container to configure.</param>
        public virtual void ConfigureContainer(TBuilder builder)
        {
        }
    }
}