using ARWNI2S.Node.Hosting;
using ARWNI2S.Node.Hosting.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace ARWNI2S.Node.Builder
{
    public class NI2SNodeBuilder
    {
        private const string EndpointRouteBuilderKey = "__EndpointRouteBuilder";
        private const string AuthenticationMiddlewareSetKey = "__AuthenticationMiddlewareSet";
        private const string AuthorizationMiddlewareSetKey = "__AuthorizationMiddlewareSet";
        private const string UseRoutingKey = "__UseRouting";

        private readonly HostApplicationBuilder _hostApplicationBuilder;
        private readonly ServiceDescriptor _genericNI2SHostServiceDescriptor;

        private NI2SNode _builtApplication;

        internal NI2SNodeBuilder(NI2SNodeOptions options, Action<IHostBuilder> configureDefaults = null)
        {
            var configuration = new ConfigurationManager();

            configuration.AddEnvironmentVariables(prefix: "ASPNETCORE_");

            _hostApplicationBuilder = new HostApplicationBuilder(new HostApplicationBuilderSettings
            {
                Args = options.Args,
                ApplicationName = options.ApplicationName,
                EnvironmentName = options.EnvironmentName,
                ContentRootPath = options.ContentRootPath,
                Configuration = configuration,
            });

            // Set WebRootPath if necessary
            if (options.WebRootPath is not null)
            {
                Configuration.AddInMemoryCollection(new[]
                {
                new KeyValuePair<string, string>(NI2SHostDefaults.WebRootKey, options.WebRootPath),
            });
            }

            // Run methods to configure web host defaults early to populate services
            var bootstrapHostBuilder = new BootstrapHostBuilder(_hostApplicationBuilder);

            // This is for testing purposes
            configureDefaults?.Invoke(bootstrapHostBuilder);

            bootstrapHostBuilder.ConfigureNI2SHostDefaults(webHostBuilder =>
            {
                // Runs inline.
                webHostBuilder.Configure(ConfigureApplication);

                InitializeNI2SHostSettings(webHostBuilder);
            },
            options =>
            {
                // We've already applied "ASPNETCORE_" environment variables to hosting config
                options.SuppressEnvironmentConfiguration = true;
            });

            _genericNI2SHostServiceDescriptor = InitializeHosting(bootstrapHostBuilder);
        }

        internal NI2SNodeBuilder(NI2SNodeOptions options, bool minimal, Action<IHostBuilder> configureDefaults = null)
        {
            Debug.Assert(minimal, "should only be called with minimal: true");

            var configuration = new ConfigurationManager();

            configuration.AddEnvironmentVariables(prefix: "ASPNETCORE_");

            // SetDefaultContentRoot needs to be added between 'ASPNETCORE_' and 'DOTNET_' in order to match behavior of the non-minimal NI2SNodeBuilder.
            SetDefaultContentRoot(options, configuration);

            // Add the default host environment variable configuration source.
            // This won't be added by CreateEmptyApplicationBuilder.
            configuration.AddEnvironmentVariables(prefix: "DOTNET_");

            _hostApplicationBuilder = Host.CreateEmptyApplicationBuilder(new HostApplicationBuilderSettings
            {
                Args = options.Args,
                ApplicationName = options.ApplicationName,
                EnvironmentName = options.EnvironmentName,
                ContentRootPath = options.ContentRootPath,
                Configuration = configuration,
            });

            // Ensure the same behavior of the non-minimal NI2SNodeBuilder by adding the default "app" Configuration sources
            ApplyDefaultAppConfigurationMinimal(_hostApplicationBuilder.Environment, configuration, options.Args);
            AddDefaultServicesMinimal(configuration, _hostApplicationBuilder.Services);

            // configure the ServiceProviderOptions here since CreateEmptyApplicationBuilder won't.
            var serviceProviderFactory = GetServiceProviderFactory(_hostApplicationBuilder);
            _hostApplicationBuilder.ConfigureContainer(serviceProviderFactory);

            // Set WebRootPath if necessary
            if (options.WebRootPath is not null)
            {
                Configuration.AddInMemoryCollection(new[]
                {
                new KeyValuePair<string, string>(NI2SHostDefaults.WebRootKey, options.WebRootPath),
            });
            }

            // Run methods to configure web host defaults early to populate services
            var bootstrapHostBuilder = new BootstrapHostBuilder(_hostApplicationBuilder);

            // This is for testing purposes
            configureDefaults?.Invoke(bootstrapHostBuilder);

            bootstrapHostBuilder.ConfigureMinimalNI2SHost(
                webHostBuilder =>
                {
                    AspNetCore.NI2SHost.ConfigureWebDefaultsMinimal(webHostBuilder);

                    // Runs inline.
                    webHostBuilder.Configure(ConfigureApplication);

                    InitializeNI2SHostSettings(webHostBuilder);
                },
                options =>
                {
                    // We've already applied "ASPNETCORE_" environment variables to hosting config
                    options.SuppressEnvironmentConfiguration = true;
                });

            _genericNI2SHostServiceDescriptor = InitializeHosting(bootstrapHostBuilder);
        }

        internal NI2SNodeBuilder(NI2SNodeOptions options, bool minimal, bool empty, Action<IHostBuilder> configureDefaults = null)
        {
            Debug.Assert(!minimal, "should only be called with minimal: false");
            Debug.Assert(empty, "should only be called with empty: true");

            var configuration = new ConfigurationManager();

            // empty builder should still default the ContentRoot as usual. This is the expected behavior for all NI2SNodeBuilders.
            SetDefaultContentRoot(options, configuration);

            _hostApplicationBuilder = Host.CreateEmptyApplicationBuilder(new HostApplicationBuilderSettings
            {
                Args = options.Args,
                ApplicationName = options.ApplicationName,
                EnvironmentName = options.EnvironmentName,
                ContentRootPath = options.ContentRootPath,
                Configuration = configuration,
            });

            // Set WebRootPath if necessary
            if (options.WebRootPath is not null)
            {
                Configuration.AddInMemoryCollection(new[]
                {
                new KeyValuePair<string, string>(NI2SHostDefaults.WebRootKey, options.WebRootPath),
            });
            }

            // Run methods to configure web host defaults early to populate services
            var bootstrapHostBuilder = new BootstrapHostBuilder(_hostApplicationBuilder);

            // This is for testing purposes
            configureDefaults?.Invoke(bootstrapHostBuilder);

            bootstrapHostBuilder.ConfigureMinimalNI2SHost(
                webHostBuilder =>
                {
                    // Note this doesn't configure any NI2SHost server - Kestrel or otherwise.
                    // It also doesn't register Routing, HostFiltering, or ForwardedHeaders.
                    // It is "empty" and up to the caller to configure these services.

                    // Runs inline.
                    webHostBuilder.Configure((context, app) => ConfigureApplication(context, app, allowDeveloperExceptionPage: false));

                    InitializeNI2SHostSettings(webHostBuilder);
                },
                options =>
                {
                    // This is an "empty" builder, so don't add the "ASPNETCORE_" environment variables
                    options.SuppressEnvironmentConfiguration = true;
                });

            _genericNI2SHostServiceDescriptor = InitializeHosting(bootstrapHostBuilder);
        }



        /// <summary>
        /// A collection of configuration providers for the application to compose. This is useful for adding new configuration sources and providers.
        /// </summary>
        public ConfigurationManager Configuration => _hostApplicationBuilder.Configuration;




        private readonly NI2SNodeOptions _options;

        internal NI2SNode Build()
        {
            throw new NotImplementedException();
        }
    }
}