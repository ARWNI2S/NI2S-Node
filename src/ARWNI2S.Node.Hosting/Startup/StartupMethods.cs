using ARWNI2S.Infrastructure.Engine.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace ARWNI2S.Node.Hosting.Startup
{
    internal sealed class StartupMethods
    {
        public StartupMethods(object instance, Action<IEngineBuilder> configure, Func<IServiceCollection, IServiceProvider> configureServices)
        {
            Debug.Assert(configure != null);
            Debug.Assert(configureServices != null);

            StartupInstance = instance;
            ConfigureDelegate = configure;
            ConfigureServicesDelegate = configureServices;
        }

        public object StartupInstance { get; }
        public Func<IServiceCollection, IServiceProvider> ConfigureServicesDelegate { get; }
        public Action<IEngineBuilder> ConfigureDelegate { get; }
    }
}