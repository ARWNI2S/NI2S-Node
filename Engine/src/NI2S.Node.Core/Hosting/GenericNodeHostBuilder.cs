using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using NI2S.Node.Builder;
using NI2S.Node.Hosting.Infrastructure;
using NI2S.Node.Hosting.Internal;
using NI2S.Node.Dummy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace NI2S.Node.Hosting
{
    internal sealed class GenericNodeHostBuilder : INodeHostBuilder, ISupportsStartup, ISupportsUseDefaultServiceProvider
    {
        private readonly IHostBuilder _builder;
        private readonly IConfiguration _config;
        private object _startupObject;
        private readonly object _startupKey = new();

        private AggregateException _hostingStartupErrors;
        private HostingStartupNodeHostBuilder _hostingStartupNodeHostBuilder;

        public GenericNodeHostBuilder(IHostBuilder builder, NodeHostBuilderOptions options)
        {
            _builder = builder;
            var configBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection();

            if (!options.SuppressEnvironmentConfiguration)
            {
                configBuilder.AddEnvironmentVariables(prefix: "DOTNET_");
            }

            _config = configBuilder.Build();

            _builder.ConfigureHostConfiguration(config =>
            {
                config.AddConfiguration(_config);

                // We do this super early but still late enough that we can process the configuration
                // wired up by calls to UseSetting
                ExecuteHostingStartups();
            });

            // IHostingStartup needs to be executed before any direct methods on the builder
            // so register these callbacks first
            _builder.ConfigureAppConfiguration((context, configurationBuilder) =>
            {
                if (_hostingStartupNodeHostBuilder != null)
                {
                    var webhostContext = GetNodeHostBuilderContext(context);
                    _hostingStartupNodeHostBuilder.ConfigureAppConfiguration(webhostContext, configurationBuilder);
                }
            });

            _builder.ConfigureServices((context, services) =>
            {
                var webhostContext = GetNodeHostBuilderContext(context);
                var nodeHostOptions = (NodeHostOptions)context.Properties[typeof(NodeHostOptions)];

                // Add the IHostingEnvironment and IApplicationLifetime from Microsoft.AspNetCore.Hosting
                services.AddSingleton(webhostContext.HostingEnvironment);
#pragma warning disable CS0618 // Type or member is obsolete
                services.AddSingleton((IHostingEnvironment)webhostContext.HostingEnvironment);
                services.AddSingleton<Microsoft.Extensions.Hosting.IApplicationLifetime, GenericNodeHostApplicationLifetime>();
#pragma warning restore CS0618 // Type or member is obsolete

                services.Configure<GenericNodeHostServiceOptions>(options =>
                {
                    // Set the options
                    options.NodeHostOptions = nodeHostOptions;
                    // Store and forward any startup errors
                    options.HostingStartupExceptions = _hostingStartupErrors;
                });

                // REVIEW: This is bad since we don't own this type. Anybody could add one of these and it would mess things up
                // We need to flow this differently
                services.TryAddSingleton(sp => new DiagnosticListener("Microsoft.AspNetCore"));
                services.TryAddSingleton<DiagnosticSource>(sp => sp.GetRequiredService<DiagnosticListener>());
                services.TryAddSingleton(sp => new ActivitySource("Microsoft.AspNetCore"));
                services.TryAddSingleton(DistributedContextPropagator.Current);

                services.TryAddSingleton<IDummyContextFactory, DefaultDummyContextFactory>();
                //services.TryAddScoped<IMiddlewareFactory, MiddlewareFactory>();
                services.TryAddSingleton<IEngineBuilderFactory, EngineBuilderFactory>();

                // IMPORTANT: This needs to run *before* direct calls on the builder (like UseStartup)
                _hostingStartupNodeHostBuilder?.ConfigureServices(webhostContext, services);

                // Support UseStartup(assemblyName)
                if (!string.IsNullOrEmpty(nodeHostOptions.StartupAssembly))
                {
                    ScanAssemblyAndRegisterStartup(context, services, webhostContext, nodeHostOptions);
                }
            });
        }

        [UnconditionalSuppressMessage("Trimmer", "IL2072", Justification = "Finding startup type in assembly requires unreferenced code. Surfaced to user in UseStartup(assemblyName).")]
        private void ScanAssemblyAndRegisterStartup(HostBuilderContext context, IServiceCollection services, NodeHostBuilderContext webhostContext, NodeHostOptions nodeHostOptions)
        {
            try
            {
                var startupType = StartupLoader.FindStartupType(nodeHostOptions.StartupAssembly!, webhostContext.HostingEnvironment.EnvironmentName);
                UseStartup(startupType, context, services);
            }
            catch (Exception ex) when (nodeHostOptions.CaptureStartupErrors)
            {
                var capture = ExceptionDispatchInfo.Capture(ex);

                services.Configure<GenericNodeHostServiceOptions>(options =>
                {
                    options.ConfigureEngine = app =>
                    {
                        // Throw if there was any errors initializing startup
                        capture.Throw();
                    };
                });
            }
        }

        private void ExecuteHostingStartups()
        {
            var nodeHostOptions = new NodeHostOptions(_config);

            if (nodeHostOptions.PreventHostingStartup)
            {
                return;
            }

            var exceptions = new List<Exception>();
            var processed = new HashSet<Assembly>();

            _hostingStartupNodeHostBuilder = new HostingStartupNodeHostBuilder(this);

            // Execute the hosting startup assemblies
            foreach (var assemblyName in nodeHostOptions.GetFinalHostingStartupAssemblies())
            {
                try
                {
                    var assembly = Assembly.Load(new AssemblyName(assemblyName));

                    if (!processed.Add(assembly))
                    {
                        // Already processed, skip it
                        continue;
                    }

                    foreach (var attribute in assembly.GetCustomAttributes<HostingStartupAttribute>())
                    {
                        var hostingStartup = (IHostingStartup)Activator.CreateInstance(attribute.HostingStartupType)!;
                        hostingStartup.Configure(_hostingStartupNodeHostBuilder);
                    }
                }
                catch (Exception ex)
                {
                    // Capture any errors that happen during startup
                    exceptions.Add(new InvalidOperationException($"Startup assembly {assemblyName} failed to execute. See the inner exception for more details.", ex));
                }
            }

            if (exceptions.Count > 0)
            {
                _hostingStartupErrors = new AggregateException(exceptions);
            }
        }

        public INodeHost Build()
        {
            throw new NotSupportedException($"Building this implementation of {nameof(INodeHostBuilder)} is not supported.");
        }

        public INodeHostBuilder ConfigureAppConfiguration(Action<NodeHostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            _builder.ConfigureAppConfiguration((context, builder) =>
            {
                var webhostBuilderContext = GetNodeHostBuilderContext(context);
                configureDelegate(webhostBuilderContext, builder);
            });

            return this;
        }

        public INodeHostBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            return ConfigureServices((context, services) => configureServices(services));
        }

        public INodeHostBuilder ConfigureServices(Action<NodeHostBuilderContext, IServiceCollection> configureServices)
        {
            _builder.ConfigureServices((context, builder) =>
            {
                var webhostBuilderContext = GetNodeHostBuilderContext(context);
                configureServices(webhostBuilderContext, builder);
            });

            return this;
        }

        public INodeHostBuilder UseDefaultServiceProvider(Action<NodeHostBuilderContext, ServiceProviderOptions> configure)
        {
            _builder.UseServiceProviderFactory(context =>
            {
                var nodeHostBuilderContext = GetNodeHostBuilderContext(context);
                var options = new ServiceProviderOptions();
                configure(nodeHostBuilderContext, options);
                return new DefaultServiceProviderFactory(options);
            });

            return this;
        }

        public INodeHostBuilder UseStartup([DynamicallyAccessedMembers(StartupLinkerOptions.Accessibility)] Type startupType)
        {
            var startupAssemblyName = startupType.Assembly.GetName().Name;

            UseSetting(NodeHostDefaults.ApplicationKey, startupAssemblyName);

            // UseStartup can be called multiple times. Only run the last one.
            _startupObject = startupType;

            var state = new UseStartupState(startupType);

            _builder.ConfigureServices((context, services) =>
            {
                // Run this delegate if the startup type matches
                if (object.ReferenceEquals(_startupObject, state.StartupType))
                {
                    UseStartup(state.StartupType, context, services);
                }
            });

            return this;
        }

        // Note: This method isn't 100% compatible with trimming. It is possible for the factory to return a derived type from TStartup.
        // RequiresUnreferencedCode isn't on this method because the majority of people won't do that.
        public INodeHostBuilder UseStartup<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] TStartup>(Func<NodeHostBuilderContext, TStartup> startupFactory)
        {
            var startupAssemblyName = startupFactory.GetMethodInfo().DeclaringType!.Assembly.GetName().Name;

            UseSetting(NodeHostDefaults.ApplicationKey, startupAssemblyName);

            // Clear the startup type
            _startupObject = startupFactory;

            _builder.ConfigureServices(ConfigureStartup);

            [UnconditionalSuppressMessage("Trimmer", "IL2072", Justification = "Startup type created by factory can't be determined statically.")]
            void ConfigureStartup(HostBuilderContext context, IServiceCollection services)
            {
                // UseStartup can be called multiple times. Only run the last one.
                if (object.ReferenceEquals(_startupObject, startupFactory))
                {
                    var nodeHostBuilderContext = GetNodeHostBuilderContext(context);
                    var instance = startupFactory(nodeHostBuilderContext) ?? throw new InvalidOperationException("The specified factory returned null startup instance.");
                    UseStartup(instance.GetType(), context, services, instance);
                }
            }

            return this;
        }

        [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2006:UnrecognizedReflectionPattern", Justification = "We need to call a generic method on IHostBuilder.")]
        private void UseStartup([DynamicallyAccessedMembers(StartupLinkerOptions.Accessibility)] Type startupType, HostBuilderContext context, IServiceCollection services, object instance = null)
        {
            var nodeHostBuilderContext = GetNodeHostBuilderContext(context);
            var nodeHostOptions = (NodeHostOptions)context.Properties[typeof(NodeHostOptions)];

            ExceptionDispatchInfo startupError = null;
            ConfigureBuilder configureBuilder = null;

            try
            {
                // We cannot support methods that return IServiceProvider as that is terminal and we need ConfigureServices to compose
                if (typeof(IStartup).IsAssignableFrom(startupType))
                {
                    throw new NotSupportedException($"{typeof(IStartup)} isn't supported");
                }
                if (StartupLoader.HasConfigureServicesIServiceProviderDelegate(startupType, context.HostingEnvironment.EnvironmentName))
                {
                    throw new NotSupportedException($"ConfigureServices returning an {typeof(IServiceProvider)} isn't supported.");
                }

                instance ??= ActivatorUtilities.CreateInstance(new HostServiceProvider(nodeHostBuilderContext), startupType);
                context.Properties[_startupKey] = instance;

                // Startup.ConfigureServices
                var configureServicesBuilder = StartupLoader.FindConfigureServicesDelegate(startupType, context.HostingEnvironment.EnvironmentName);
                var configureServices = configureServicesBuilder.Build(instance);

                configureServices(services);

                // REVIEW: We're doing this in the callback so that we have access to the hosting environment
                // Startup.ConfigureContainer
                var configureContainerBuilder = StartupLoader.FindConfigureContainerDelegate(startupType, context.HostingEnvironment.EnvironmentName);
                if (configureContainerBuilder.MethodInfo != null)
                {
                    var containerType = configureContainerBuilder.GetContainerType();
                    // Store the builder in the property bag
                    _builder.Properties[typeof(ConfigureContainerBuilder)] = configureContainerBuilder;

                    var actionType = typeof(Action<,>).MakeGenericType(typeof(HostBuilderContext), containerType);

                    // Get the private ConfigureContainer method on this type then close over the container type
                    var configureCallback = typeof(GenericNodeHostBuilder).GetMethod(nameof(ConfigureContainerImpl), BindingFlags.NonPublic | BindingFlags.Instance)!
                                                     .MakeGenericMethod(containerType)
                                                     .CreateDelegate(actionType, this);

                    // _builder.ConfigureContainer<T>(ConfigureContainer);
                    typeof(IHostBuilder).GetMethod(nameof(IHostBuilder.ConfigureContainer))!
                        .MakeGenericMethod(containerType)
                        .InvokeWithoutWrappingExceptions(_builder, new object[] { configureCallback });
                }

                // Resolve Configure after calling ConfigureServices and ConfigureContainer
                configureBuilder = StartupLoader.FindConfigureDelegate(startupType, context.HostingEnvironment.EnvironmentName);
            }
            catch (Exception ex) when (nodeHostOptions.CaptureStartupErrors)
            {
                startupError = ExceptionDispatchInfo.Capture(ex);
            }

            // Startup.Configure
            services.Configure<GenericNodeHostServiceOptions>(options =>
            {
                options.ConfigureEngine = app =>
                {
                    // Throw if there was any errors initializing startup
                    startupError?.Throw();

                    // Execute Startup.Configure
                    if (instance != null && configureBuilder != null)
                    {
                        configureBuilder.Build(instance)(app);
                    }
                };
            });
        }

        private void ConfigureContainerImpl<TContainer>(HostBuilderContext context, TContainer container) where TContainer : notnull
        {
            var instance = context.Properties[_startupKey];
            var builder = (ConfigureContainerBuilder)context.Properties[typeof(ConfigureContainerBuilder)];
            builder.Build(instance)(container);
        }

        public INodeHostBuilder Configure(Action<IEngineBuilder> configure)
        {
            var startupAssemblyName = configure.GetMethodInfo().DeclaringType!.Assembly.GetName().Name!;

            UseSetting(NodeHostDefaults.ApplicationKey, startupAssemblyName);

            // Clear the startup type
            _startupObject = configure;

            _builder.ConfigureServices((context, services) =>
            {
                if (object.ReferenceEquals(_startupObject, configure))
                {
                    services.Configure<GenericNodeHostServiceOptions>(options =>
                    {
                        options.ConfigureEngine = app => configure(app);
                    });
                }
            });

            return this;
        }

        public INodeHostBuilder Configure(Action<NodeHostBuilderContext, IEngineBuilder> configure)
        {
            var startupAssemblyName = configure.GetMethodInfo().DeclaringType!.Assembly.GetName().Name!;

            UseSetting(NodeHostDefaults.ApplicationKey, startupAssemblyName);

            // Clear the startup type
            _startupObject = configure;

            _builder.ConfigureServices((context, services) =>
            {
                if (object.ReferenceEquals(_startupObject, configure))
                {
                    services.Configure<GenericNodeHostServiceOptions>(options =>
                    {
                        var webhostBuilderContext = GetNodeHostBuilderContext(context);
                        options.ConfigureEngine = app => configure(webhostBuilderContext, app);
                    });
                }
            });

            return this;
        }

        private NodeHostBuilderContext GetNodeHostBuilderContext(HostBuilderContext context)
        {
            if (!context.Properties.TryGetValue(typeof(NodeHostBuilderContext), out var contextVal))
            {
                // Use _config as a fallback for NodeHostOptions in case the chained source was removed from the hosting IConfigurationBuilder.
                var options = new NodeHostOptions(context.Configuration, fallbackConfiguration: _config, environment: context.HostingEnvironment);
                var nodeHostBuilderContext = new NodeHostBuilderContext
                {
                    Configuration = context.Configuration,
                    HostingEnvironment = new HostingEnvironment(),
                };
                //nodeHostBuilderContext.HostingEnvironment.Initialize(context.HostingEnvironment.ContentRootPath, options, baseEnvironment: context.HostingEnvironment);
                context.Properties[typeof(NodeHostBuilderContext)] = nodeHostBuilderContext;
                context.Properties[typeof(NodeHostOptions)] = options;
                return nodeHostBuilderContext;
            }

            // Refresh config, it's periodically updated/replaced
            var nodeHostContext = (NodeHostBuilderContext)contextVal;
            nodeHostContext.Configuration = context.Configuration;
            return nodeHostContext;
        }

        public string GetSetting(string key)
        {
            return _config[key];
        }

        public INodeHostBuilder UseSetting(string key, string value)
        {
            _config[key] = value;
            return this;
        }

        // This exists just so that we can use ActivatorUtilities.CreateInstance on the Startup class
        private sealed class HostServiceProvider : IServiceProvider
        {
            private readonly NodeHostBuilderContext _context;

            public HostServiceProvider(NodeHostBuilderContext context)
            {
                _context = context;
            }

            public object GetService(Type serviceType)
            {
                // The implementation of the HostingEnvironment supports both interfaces
#pragma warning disable CS0618 // Type or member is obsolete
                if (serviceType == typeof(Microsoft.Extensions.Hosting.IHostingEnvironment)
                    || serviceType == typeof(IHostingEnvironment)
#pragma warning restore CS0618 // Type or member is obsolete
                        || serviceType == typeof(INodeHostEnvironment)
                    || serviceType == typeof(IHostEnvironment)
                    )
                {
                    return _context.HostingEnvironment;
                }

                if (serviceType == typeof(IConfiguration))
                {
                    return _context.Configuration;
                }

                return null;
            }
        }
    }
}