// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.


using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using NI2S.Node.Engine;
using NI2S.Node.Hosting;
using NI2S.Node.Hosting.Builder;
using System;
using System.Buffers;
using System.Linq;

namespace NI2S.Node.Hosting
{
    /// <summary>
    /// Extension methods for setting up essential NI2S services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class NI2SCoreServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the minimum essential NI2S services to the specified <see cref="IServiceCollection" />. Additional services
        /// including NI2S's support for authorization, formatters, and validation must be added separately using the
        /// <see cref="INI2SCoreBuilder"/> returned from this method.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>An <see cref="INI2SCoreBuilder"/> that can be used to further configure the NI2S services.</returns>
        /// <remarks>
        /// The <see cref="NI2SCoreServiceCollectionExtensions.AddNI2SCore(IServiceCollection)"/> approach for configuring
        /// NI2S is provided for experienced NI2S developers who wish to have full control over the set of default services
        /// registered. <see cref="NI2SCoreServiceCollectionExtensions.AddNI2SCore(IServiceCollection)"/> will register
        /// the minimum set of services necessary to route requests and invoke controllers. It is not expected that any
        /// engine will satisfy its requirements with just a call to
        /// <see cref="NI2SCoreServiceCollectionExtensions.AddNI2SCore(IServiceCollection)"/>. Additional configuration using the
        /// <see cref="INI2SCoreBuilder"/> will be required.
        /// </remarks>
        public static INI2SCoreBuilder AddNI2SCore(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            var environment = GetServiceFromCollection<INodeHostEnvironment>(services);
            var moduleManager = GetModuleManager(services, environment);
            services.TryAddSingleton(moduleManager);

            ConfigureDefaultModuleProviders(moduleManager);
            ConfigureDefaultServices(services);
            AddNI2SCoreServices(services);

            var builder = new NI2SCoreBuilder(services, moduleManager);

            return builder;
        }

        /* 002.3.2.3 - ConfigureNodeEngineBuilder(...) -> builder.Services.ConfigureEngineServices(...) -> services.AddNI2SCore() 
                       -> ConfigureDefaultModuleProviders(...) */
        private static void ConfigureDefaultModuleProviders(IModuleManager manager)
        {
            // TODO: apply required corrections.
            //if (!manager.Providers.OfType<ControllerModuleProvider>().Any())
            //{
            //    manager.Providers.Add(new ControllerModuleProvider());
            //}
        }

        /* 002.3.2.2 - ConfigureNodeEngineBuilder(...) -> builder.Services.ConfigureEngineServices(...) -> services.AddNI2SCore() 
                       -> GetModuleManager(...) */
        private static IModuleManager GetModuleManager(IServiceCollection services, INodeHostEnvironment environment)
        {
            var manager = GetServiceFromCollection<ModuleManager>(services);
            if (manager == null)
            {
                manager = new ModuleManager();

                var entryAssemblyName = environment?.ApplicationName;
                if (string.IsNullOrEmpty(entryAssemblyName))
                {
                    return manager;
                }

                manager.PopulateDefaultParts(entryAssemblyName);
            }

            return manager;
        }

        private static T GetServiceFromCollection<T>(IServiceCollection services)
        {
            return (T)services
                .LastOrDefault(d => d.ServiceType == typeof(T))
                ?.ImplementationInstance;
        }

        /// <summary>
        /// Adds the minimum essential NI2S services to the specified <see cref="IServiceCollection" />. Additional services
        /// including NI2S's support for authorization, formatters, and validation must be added separately using the
        /// <see cref="INI2SCoreBuilder"/> returned from this method.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">An <see cref="Action{NI2SOptions}"/> to configure the provided <see cref="NI2SOptions"/>.</param>
        /// <returns>An <see cref="INI2SCoreBuilder"/> that can be used to further configure the NI2S services.</returns>
        /// <remarks>
        /// The <see cref="NI2SCoreServiceCollectionExtensions.AddNI2SCore(IServiceCollection)"/> approach for configuring
        /// NI2S is provided for experienced NI2S developers who wish to have full control over the set of default services
        /// registered. <see cref="NI2SCoreServiceCollectionExtensions.AddNI2SCore(IServiceCollection)"/> will register
        /// the minimum set of services necessary to route requests and invoke controllers. It is not expected that any
        /// engine will satisfy its requirements with just a call to
        /// <see cref="NI2SCoreServiceCollectionExtensions.AddNI2SCore(IServiceCollection)"/>. Additional configuration using the
        /// <see cref="INI2SCoreBuilder"/> will be required.
        /// </remarks>
        public static INI2SCoreBuilder AddNI2SCore(
            this IServiceCollection services,
            Action<NI2SOptions> setupAction)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(setupAction);

            var builder = services.AddNI2SCore();
            services.Configure(setupAction);

            return builder;
        }

        // To enable unit testing
        /* 002.3.2.5 - ConfigureNodeEngineBuilder(...) -> builder.Services.ConfigureEngineServices(...) -> services.AddNI2SCore() 
                       -> AddNI2SCoreServices(...) */
        internal static void AddNI2SCoreServices(IServiceCollection services)
        {
            //
            // Options
            //
            // TODO: apply required Options.
            //services.TryAddEnumerable(
            //    ServiceDescriptor.Transient<IConfigureOptions<NI2SOptions>, NI2SCoreNI2SOptionsSetup>());
            //services.TryAddEnumerable(
            //    ServiceDescriptor.Transient<IPostConfigureOptions<NI2SOptions>, NI2SCoreNI2SOptionsSetup>());
            //services.TryAddEnumerable(
            //    ServiceDescriptor.Transient<IConfigureOptions<ApiBehaviorOptions>, ApiBehaviorOptionsSetup>());
            //services.TryAddEnumerable(
            //    ServiceDescriptor.Transient<IConfigureOptions<RouteOptions>, NI2SCoreRouteOptionsSetup>());

            //
            // Action Discovery
            //
            // These are consumed only when creating action descriptors, then they can be deallocated
            // TODO: apply required corrections.
            //services.TryAddSingleton<ApplicationModelFactory>();
            //services.TryAddEnumerable(
            //    ServiceDescriptor.Transient<IApplicationModelProvider, DefaultApplicationModelProvider>());
            //services.TryAddEnumerable(
            //    ServiceDescriptor.Transient<IApplicationModelProvider, ApiBehaviorApplicationModelProvider>());
            //services.TryAddEnumerable(
            //    ServiceDescriptor.Transient<IActionDescriptorProvider, ControllerActionDescriptorProvider>());

            //services.TryAddSingleton<IActionDescriptorCollectionProvider, DefaultActionDescriptorCollectionProvider>();

            //
            // Action Selection
            //
            // TODO: apply required corrections.
            //services.TryAddSingleton<IActionSelector, ActionSelector>();
            //services.TryAddSingleton<ActionConstraintCache>();

            // Will be cached by the DefaultActionSelector
            // TODO: apply required corrections.
            //services.TryAddEnumerable(ServiceDescriptor.Transient<IActionConstraintProvider, DefaultActionConstraintProvider>());

            // Policies for Endpoints
            // TODO: apply required corrections.
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, ActionConstraintMatcherPolicy>());

            //
            // Controller Factory
            //
            // This has a cache, so it needs to be a singleton
            // TODO: apply required corrections.
            //services.TryAddSingleton<IControllerFactory, DefaultControllerFactory>();

            // Will be cached by the DefaultControllerFactory
            // TODO: apply required corrections.
            //services.TryAddTransient<IControllerActivator, DefaultControllerActivator>();

            //services.TryAddSingleton<IControllerFactoryProvider, ControllerFactoryProvider>();
            //services.TryAddSingleton<IControllerActivatorProvider, ControllerActivatorProvider>();
            //services.TryAddEnumerable(
            //    ServiceDescriptor.Transient<IControllerPropertyActivator, DefaultControllerPropertyActivator>());

            //
            // Action Invoker
            //
            // The IActionInvokerFactory is cachable
            // TODO: apply required corrections.
            //services.TryAddSingleton<IActionInvokerFactory, ActionInvokerFactory>();
            //services.TryAddEnumerable(
            //    ServiceDescriptor.Transient<IActionInvokerProvider, ControllerActionInvokerProvider>());

            // These are stateless
            // TODO: apply required corrections.
            //services.TryAddSingleton<ControllerActionInvokerCache>();
            //services.TryAddEnumerable(
            //    ServiceDescriptor.Singleton<IFilterProvider, DefaultFilterProvider>());
            //services.TryAddSingleton<IActionResultTypeMapper, ActionResultTypeMapper>();

            //
            // Request body limit filters
            //
            // TODO: apply required corrections.
            //services.TryAddTransient<RequestSizeLimitFilter>();
            //services.TryAddTransient<DisableRequestSizeLimitFilter>();
            //services.TryAddTransient<RequestFormLimitsFilter>();

            //
            // ModelBinding, Validation
            //
            // The DefaultModelMetadataProvider does significant caching and should be a singleton.
            // TODO: apply required corrections.
            //services.TryAddSingleton<IModelMetadataProvider, DefaultModelMetadataProvider>();
            //services.TryAdd(ServiceDescriptor.Transient<ICompositeMetadataDetailsProvider>(s =>
            //{
            //    var options = s.GetRequiredService<IOptions<NI2SOptions>>().Value;
            //    return new DefaultCompositeMetadataDetailsProvider(options.ModelMetadataDetailsProviders);
            //}));
            //services.TryAddSingleton<IModelBinderFactory, ModelBinderFactory>();
            //services.TryAddSingleton<IObjectModelValidator>(s =>
            //{
            //    var options = s.GetRequiredService<IOptions<NI2SOptions>>().Value;
            //    var metadataProvider = s.GetRequiredService<IModelMetadataProvider>();
            //    return new DefaultObjectValidator(metadataProvider, options.ModelValidatorProviders, options);
            //});
            //services.TryAddSingleton<ClientValidatorCache>();
            //services.TryAddSingleton<ParameterBinder>();

            //
            // Random Infrastructure
            //
            // TODO: apply required corrections.
            //services.TryAddSingleton<NI2SMarkerService, NI2SMarkerService>();
            //services.TryAddSingleton<ITypeActivatorCache, TypeActivatorCache>();
            //services.TryAddSingleton<IUrlHelperFactory, UrlHelperFactory>();
            //services.TryAddSingleton<IHttpRequestStreamReaderFactory, MemoryPoolHttpRequestStreamReaderFactory>();
            //services.TryAddSingleton<IHttpResponseStreamWriterFactory, MemoryPoolHttpResponseStreamWriterFactory>();
            services.TryAddSingleton(ArrayPool<byte>.Shared);
            services.TryAddSingleton(ArrayPool<char>.Shared);
            //services.TryAddSingleton<OutputFormatterSelector, DefaultOutputFormatterSelector>();
            //services.TryAddSingleton<IActionResultExecutor<ObjectResult>, ObjectResultExecutor>();
            //services.TryAddSingleton<IActionResultExecutor<PhysicalFileResult>, PhysicalFileResultExecutor>();
            //services.TryAddSingleton<IActionResultExecutor<VirtualFileResult>, VirtualFileResultExecutor>();
            //services.TryAddSingleton<IActionResultExecutor<FileStreamResult>, FileStreamResultExecutor>();
            //services.TryAddSingleton<IActionResultExecutor<FileContentResult>, FileContentResultExecutor>();
            //services.TryAddSingleton<IActionResultExecutor<RedirectResult>, RedirectResultExecutor>();
            //services.TryAddSingleton<IActionResultExecutor<LocalRedirectResult>, LocalRedirectResultExecutor>();
            //services.TryAddSingleton<IActionResultExecutor<RedirectToActionResult>, RedirectToActionResultExecutor>();
            //services.TryAddSingleton<IActionResultExecutor<RedirectToRouteResult>, RedirectToRouteResultExecutor>();
            //services.TryAddSingleton<IActionResultExecutor<RedirectToPageResult>, RedirectToPageResultExecutor>();
            //services.TryAddSingleton<IActionResultExecutor<ContentResult>, ContentResultExecutor>();
            //services.TryAddSingleton<IActionResultExecutor<JsonResult>, SystemTextJsonResultExecutor>();
            //services.TryAddSingleton<IClientErrorFactory, ProblemDetailsClientErrorFactory>();

            //
            // Route Handlers
            //
            // TODO: apply required corrections.
            //services.TryAddSingleton<NI2SRouteHandler>(); // Only one per app
            //services.TryAddTransient<NI2SAttributeRouteHandler>(); // Many per app

            //
            // Endpoint Routing / Endpoints
            //
            // TODO: apply required corrections.
            //services.TryAddSingleton<ControllerActionEndpointDataSourceFactory>();
            //services.TryAddSingleton<OrderedEndpointsSequenceProviderCache>();
            //services.TryAddSingleton<ControllerActionEndpointDataSourceIdProvider>();
            //services.TryAddSingleton<ActionEndpointFactory>();
            //services.TryAddSingleton<DynamicControllerEndpointSelectorCache>();
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, DynamicControllerEndpointMatcherPolicy>());
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<IRequestDelegateFactory, ControllerRequestDelegateFactory>());

            //
            // Middleware pipeline filter related
            //
            // TODO: apply required corrections.
            //services.TryAddSingleton<MiddlewareFilterConfigurationProvider>();
            // This maintains a cache of middleware pipelines, so it needs to be a singleton
            //services.TryAddSingleton<MiddlewareFilterBuilder>();
            // Sets ApplicationBuilder on MiddlewareFilterBuilder
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<IStartupFilter, MiddlewareFilterBuilderStartupFilter>());

            // ProblemDetails
            // TODO: apply required corrections.
            //services.TryAddSingleton<ProblemDetailsFactory, DefaultProblemDetailsFactory>();
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<IProblemDetailsWriter, DefaultApiProblemDetailsWriter>());
        }

        /* 002.3.2.4 - ConfigureNodeEngineBuilder(...) -> builder.Services.ConfigureEngineServices(...) -> services.AddNI2SCore() 
                       -> ConfigureDefaultServices(...) */
        private static void ConfigureDefaultServices(IServiceCollection services)
        {
            // TODO: apply required corrections.
            //services.AddRouting();
        }
    }
}