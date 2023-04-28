// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NI2S.Node.Core.Configuration;
using NI2S.Node.Core.Infrastructure;
using NI2S.Node.Hosting;
using NI2S.Node.Hosting.Builder;
using NI2S.Node.Infrastructure.Extensions;
using System.Threading.Tasks;

namespace NI2S.Node
{
    /// <summary>
    /// Provides access to NI2S Node configuration and execution.
    /// </summary>
    public static class NI2SNode
    {
        public static async Task RunAsync(string[] args)
        {
            NodeEngineHost engineHost = CreateDefaultNodeEngineBuilder(args).Build();

            //configure the engine HTTP request pipeline
            engineHost.ConfigureEventHandler();
            await engineHost.StartEngineAsync();

            await engineHost.RunAsync();
        }

        private static NodeEngineHostBuilder CreateDefaultNodeEngineBuilder(string[] args) => ConfigureNodeEngineBuilder(NodeEngineHost.CreateBuilder(args));

        /* 002 - ConfigureNodeEngineBuilder(...) */
        private static NodeEngineHostBuilder ConfigureNodeEngineBuilder(NodeEngineHostBuilder builder)
        {
            builder.Configuration.AddJsonFile(ConfigurationDefaults.NodeSettingsFilePath, true, true);
            if (!string.IsNullOrEmpty(builder.Environment?.EnvironmentName))
            {
                var path = string.Format(ConfigurationDefaults.NodeSettingsEnvironmentFilePath, builder.Environment.EnvironmentName);
                builder.Configuration.AddJsonFile(path, true, true);
            }
            builder.Configuration.AddEnvironmentVariables();

            //load engine settings
            builder.Services.ConfigureEngineSettings(builder);

            var appSettings = Singleton<NodeSettings>.Instance;
            var useAutofac = appSettings.Get<CommonConfig>().UseAutofac;

            if (useAutofac)
                builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            else
                builder.Host.UseDefaultServiceProvider(options =>
                {
                    //we don't validate the scopes, since at the app start and the initial configuration we need 
                    //to resolve some services (registered as "scoped") through the root container
                    options.ValidateScopes = false;
                    options.ValidateOnBuild = true;
                });

            //add services to the engine and configure service provider
            builder.Services.ConfigureEngineServices(builder);

            return builder;
        }
    }
}
