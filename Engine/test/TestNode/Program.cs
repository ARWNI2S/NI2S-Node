// TODO: find and replace this line with COPYRIGTH NOTICE entire solution

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
            var builder = WebApplication.CreateBuilder(args);
            //var builder = Host.CreateApplicationBuilder(args);

            //builder.Configuration.AddJsonFile(NopConfigurationDefaults.AppSettingsFilePath, true, true);
            builder.Configuration.AddJsonFile(NodeConfigurationDefaults.AppSettingsFilePath, true, true);
            if (!string.IsNullOrEmpty(builder.Environment?.EnvironmentName))
            {
                //var path = string.Format(NopConfigurationDefaults.AppSettingsEnvironmentFilePath, builder.Environment.EnvironmentName);
                var path = string.Format(NodeConfigurationDefaults.AppSettingsEnvironmentFilePath, builder.Environment.EnvironmentName);
                builder.Configuration.AddJsonFile(path, true, true);
            }
            //builder.Configuration.AddEnvironmentVariables();
            builder.Configuration.AddEnvironmentVariables();

            //load application settings
            //builder.Services.ConfigureApplicationSettings(builder);
            builder.Services.ConfigureApplicationSettings(builder);

            //var appSettings = Singleton<AppSettings>.Instance;
            var appSettings = Singleton<AppSettings>.Instance;
            //var useAutofac = appSettings.Get<CommonConfig>().UseAutofac;
            var useAutofac = appSettings.Get<CommonConfig>().UseAutofac;

            //if (useAutofac)
            //builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            //else
            //builder.Host.UseDefaultServiceProvider(options =>
            //{
            //we don't validate the scopes, since at the app start and the initial configuration we need 
            //to resolve some services (registered as "scoped") through the root container
            //options.ValidateScopes = false;
            //options.ValidateOnBuild = true;
            //});
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
            //builder.Services.ConfigureApplicationServices(builder);
            builder.Services.ConfigureApplicationServices(builder);

            //var app = builder.Build();
            var app = builder.Build();

            //configure the application HTTP request pipeline
            //app.ConfigureRequestPipeline();
            app.ConfigureRequestPipeline();
            //await app.StartEngineAsync();
            await app.StartEngineAsync();

            //await app.RunAsync();
            await app.RunAsync();
        }

        //static void Main(string[] args)
        //{
        //    //var builder = NodeHost.CreateHostBuilder(args);
        //    var builder2 = Host.CreateDefaultBuilder(args);
        //    //builder.UseOrleans(siloBuilder => siloBuilder.UseLocalhostClustering())
        //    //var app = builder.Build();
        //    var app2 = builder2.Build();
        //}
    }
}