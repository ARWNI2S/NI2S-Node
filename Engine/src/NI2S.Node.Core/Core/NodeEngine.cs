// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NI2S.Node.Hosting;
using NI2S.Node.Hosting.Builder;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

namespace NI2S.Node.Core
{
    /// <summary>
    /// Provides convenience methods for creating instances of <see cref="INodeHost"/> and <see cref="INodeHostBuilder"/> with pre-configured defaults.
    /// </summary>
    public static class NodeEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeHostBuilder"/> class with pre-configured defaults.
        /// </summary>
        /// <remarks>
        ///   The following defaults are applied to the returned <see cref="NodeHostBuilder"/>:
        ///     use Kestrel as the web server and configure it using the application's configuration providers,
        ///     set the <see cref="IHostEnvironment.ContentRootPath"/> to the result of <see cref="Directory.GetCurrentDirectory()"/>,
        ///     load <see cref="IConfiguration"/> from 'appsettings.json' and 'appsettings.[<see cref="IHostEnvironment.EnvironmentName"/>].json',
        ///     load <see cref="IConfiguration"/> from User Secrets when <see cref="IHostEnvironment.EnvironmentName"/> is 'Development' using the entry assembly,
        ///     load <see cref="IConfiguration"/> from environment variables,
        ///     configure the <see cref="ILoggerFactory"/> to log to the console and debug output,
        ///     adds the HostFiltering middleware,
        ///     adds the ForwardedHeaders middleware if ASPNETCORE_FORWARDEDHEADERS_ENABLED=true,
        ///     and enable IIS integration.
        /// </remarks>
        /// <returns>The initialized <see cref="INodeHostBuilder"/>.</returns>
        public static INodeHostBuilder CreateDefaultBuilder() =>
            CreateDefaultBuilder(args: null!);

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeHostBuilder"/> class with pre-configured defaults.
        /// </summary>
        /// <remarks>
        ///   The following defaults are applied to the returned <see cref="NodeHostBuilder"/>:
        ///     use Kestrel as the web server and configure it using the application's configuration providers,
        ///     set the <see cref="IHostEnvironment.ContentRootPath"/> to the result of <see cref="Directory.GetCurrentDirectory()"/>,
        ///     load <see cref="IConfiguration"/> from 'appsettings.json' and 'appsettings.[<see cref="IHostEnvironment.EnvironmentName"/>].json',
        ///     load <see cref="IConfiguration"/> from User Secrets when <see cref="IHostEnvironment.EnvironmentName"/> is 'Development' using the entry assembly,
        ///     load <see cref="IConfiguration"/> from environment variables,
        ///     load <see cref="IConfiguration"/> from supplied command line args,
        ///     configure the <see cref="ILoggerFactory"/> to log to the console and debug output,
        ///     configure the <see cref="INodeHostEnvironment.WebRootFileProvider"/> to map static web assets when <see cref="IHostEnvironment.EnvironmentName"/> is 'Development' using the entry assembly,
        ///     adds the HostFiltering middleware,
        ///     adds the ForwardedHeaders middleware if ASPNETCORE_FORWARDEDHEADERS_ENABLED=true,
        ///     and enable IIS integration.
        /// </remarks>
        /// <param name="args">The command line args.</param>
        /// <returns>The initialized <see cref="INodeHostBuilder"/>.</returns>
        public static INodeHostBuilder CreateDefaultBuilder(string[] args)
        {
            var builder = new NodeHostBuilder();

            if (string.IsNullOrEmpty(builder.GetSetting(NodeHostDefaults.ContentRootKey)))
            {
                builder.UseContentRoot(Directory.GetCurrentDirectory());
            }
            if (args != null)
            {
                builder.UseConfiguration(new ConfigurationBuilder().AddCommandLine(args).Build());
            }

            builder.ConfigureNodeConfiguration((hostingContext, config) =>
            {
                var env = hostingContext.HostingEnvironment;

                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                      .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                if (env.IsDevelopment())
                {
                    if (!string.IsNullOrEmpty(env.ApplicationName))
                    {
                        var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                        if (appAssembly != null)
                        {
                            config.AddUserSecrets(appAssembly, optional: true);
                        }
                    }
                }

                config.AddEnvironmentVariables();

                if (args != null)
                {
                    config.AddCommandLine(args);
                }
            })
            .ConfigureLogging((hostingContext, loggingBuilder) =>
            {
                loggingBuilder.Configure(options =>
                {
                    options.ActivityTrackingOptions = ActivityTrackingOptions.SpanId
                                                        | ActivityTrackingOptions.TraceId
                                                        | ActivityTrackingOptions.ParentId;
                });
                loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
                loggingBuilder.AddEventSourceLogger();
            }).
            UseDefaultServiceProvider((context, options) =>
            {
                options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
            });

            ConfigureNodeDefaults(builder);

            return builder;
        }

        /* 006 */
        internal static void ConfigureNodeDefaults(INodeHostBuilder builder)
        {
            builder.ConfigureNodeConfiguration((ctx, cb) =>
            {
                if (ctx.HostingEnvironment.IsDevelopment())
                {
                    StaticAssetsLoader.UseStaticAssets(ctx.HostingEnvironment, ctx.Configuration);
                }
            });

            builder.UseNI2S((builderContext, options) =>
            {
                //options.Configure(builderContext.Configuration.GetSection("nodeengine"), reloadOnChange: true);
            });




            //builder.UseSocketServer((builderContext, options) =>
            //{
            //    options.Configure(builderContext.Configuration.GetSection("SocketServer"), reloadOnChange: true);
            //})
            //.ConfigureServices((hostingContext, services) =>
            //{
                // Fallback
                //services.PostConfigure<HostFilteringOptions>(options =>
                //{
                //    if (options.AllowedHosts == null || options.AllowedHosts.Count == 0)
                //    {
                //        // "AllowedHosts": "localhost;127.0.0.1;[::1]"
                //        var hosts = hostingContext.Configuration["AllowedHosts"]?.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                //        // Fall back to "*" to disable.
                //        options.AllowedHosts = (hosts?.Length > 0 ? hosts : new[] { "*" });
                //    }
                //});
                //// Change notification
                //services.AddSingleton<IOptionsChangeTokenSource<HostFilteringOptions>>(
                //        new ConfigurationChangeTokenSource<HostFilteringOptions>(hostingContext.Configuration));

                //services.AddTransient<IStartupFilter, HostFilteringStartupFilter>();
                //services.AddTransient<IStartupFilter, ForwardedHeadersStartupFilter>();
                //services.AddTransient<IConfigureOptions<ForwardedHeadersOptions>, ForwardedHeadersOptionsSetup>();

                //services.AddRouting();
            //})
            //.UseIIS()
            //.UseIISIntegration()
            //;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeHostBuilder"/> class with pre-configured defaults using typed Startup.
        /// </summary>
        /// <remarks>
        ///   The following defaults are applied to the returned <see cref="NodeHostBuilder"/>:
        ///     use Kestrel as the web server and configure it using the application's configuration providers,
        ///     set the <see cref="IHostEnvironment.ContentRootPath"/> to the result of <see cref="Directory.GetCurrentDirectory()"/>,
        ///     load <see cref="IConfiguration"/> from 'appsettings.json' and 'appsettings.[<see cref="IHostEnvironment.EnvironmentName"/>].json',
        ///     load <see cref="IConfiguration"/> from User Secrets when <see cref="IHostEnvironment.EnvironmentName"/> is 'Development' using the entry assembly,
        ///     load <see cref="IConfiguration"/> from environment variables,
        ///     load <see cref="IConfiguration"/> from supplied command line args,
        ///     configure the <see cref="ILoggerFactory"/> to log to the console and debug output,
        ///     enable IIS integration.
        /// </remarks>
        /// <typeparam name ="TStartup">The type containing the startup methods for the application.</typeparam>
        /// <param name="args">The command line args.</param>
        /// <returns>The initialized <see cref="INodeHostBuilder"/>.</returns>
        public static INodeHostBuilder CreateDefaultBuilder<[DynamicallyAccessedMembers(StartupLinkerOptions.Accessibility)] TStartup>(string[] args) where TStartup : class =>
            CreateDefaultBuilder(args).UseStartup<TStartup>();
    }

}
