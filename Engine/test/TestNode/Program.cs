// TODO: find and replace this line with COPYRIGTH NOTICE entire solution

using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NI2S.Node.Core.Configuration;
using NI2S.Node.Core.Infrastructure;
using NI2S.Node.Hosting;
using NI2S.Node.Infrastructure.Extensions;

namespace TestNode
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = NodeEngine.CreateBuilder(args);

            builder.Configuration.AddJsonFile(NodeConfigurationDefaults.AppSettingsFilePath, true, true);
            if (!string.IsNullOrEmpty(builder.Environment?.EnvironmentName))
            {
                var path = string.Format(NodeConfigurationDefaults.AppSettingsEnvironmentFilePath, builder.Environment.EnvironmentName);
                builder.Configuration.AddJsonFile(path, true, true);
            }
            builder.Configuration.AddEnvironmentVariables();

            //load application settings
            builder.Services.ConfigureEngineSettings(builder);

            var appSettings = Singleton<AppSettings>.Instance;
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

            //add services to the application and configure service provider
            builder.Services.ConfigureEngineServices(builder);

            var app = builder.Build();

            //configure the application HTTP request pipeline
            app.ConfigureRequestPipeline();
            await app.StartEngineAsync();

            await app.RunAsync();
        }
    }
}