using ARWNI2S.Core.Engine.Builder;
using ARWNI2S.Core.Engine.Parts;
using ARWNI2S.Core.Infrastructure;
using ARWNI2S.Engine;
using ARWNI2S.Engine.Builder;
using ARWNI2S.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ARWNI2S.Core.Engine
{
    public static class NI2SCoreServiceCollectionExtensions
    {
        public static INiisCoreBuilder AddNI2SCore(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services, "services");
            INiisHostEnvironment serviceFromCollection = GetServiceFromCollection<INiisHostEnvironment>(services);
            ApplicationPartManager applicationPartManager = GetApplicationPartManager(services, serviceFromCollection);
            services.TryAddSingleton(applicationPartManager);
            ConfigureDefaultFeatureProviders(applicationPartManager);
            ConfigureDefaultServices(services);
            AddNI2SCoreServices(services);
            return new NI2SCoreBuilder(services, applicationPartManager);
        }

        private static void ConfigureDefaultFeatureProviders(ApplicationPartManager manager)
        {
            //if (!manager.FeatureProviders.OfType<ControllerFeatureProvider>().Any())
            //{
            //    manager.FeatureProviders.Add(new ControllerFeatureProvider());
            //}
        }

        private static ApplicationPartManager GetApplicationPartManager(IServiceCollection services, INiisHostEnvironment environment)
        {
            ApplicationPartManager applicationPartManager = GetServiceFromCollection<ApplicationPartManager>(services);
            if (applicationPartManager == null)
            {
                applicationPartManager = new ApplicationPartManager();
                string text = environment?.ApplicationName;
                if (string.IsNullOrEmpty(text))
                {
                    return applicationPartManager;
                }
                applicationPartManager.PopulateDefaultParts(text);
            }
            return applicationPartManager;
        }

        private static T GetServiceFromCollection<T>(IServiceCollection services)
        {
            return (T)(services.LastOrDefault((ServiceDescriptor d) => d.ServiceType == typeof(T))?.ImplementationInstance);
        }

        //public static INiisCoreBuilder AddNI2SCore(this IServiceCollection services, Action<MvcOptions> setupAction)
        //{
        //    ArgumentNullException.ThrowIfNull(services, "services");
        //    ArgumentNullException.ThrowIfNull(setupAction, "setupAction");
        //    INiisCoreBuilder result = services.AddNI2SCore();
        //    services.Configure(setupAction);
        //    return result;
        //}

        internal static void AddNI2SCoreServices(IServiceCollection services)
        {
            services.TryAddSingleton<INiisFileProvider, NI2SFileProvider>();
            //services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, MvcCoreMvcOptionsSetup>());
            //services.TryAddEnumerable(ServiceDescriptor.Transient<IPostConfigureOptions<MvcOptions>, MvcCoreMvcOptionsSetup>());
            //services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<ApiBehaviorOptions>, ApiBehaviorOptionsSetup>());
            //services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<RouteOptions>, MvcCoreRouteOptionsSetup>());
            //services.TryAddSingleton<ApplicationModelFactory>();
            //services.TryAddEnumerable(ServiceDescriptor.Transient<IApplicationModelProvider, DefaultApplicationModelProvider>());
            //services.TryAddEnumerable(ServiceDescriptor.Transient<IApplicationModelProvider, ApiBehaviorApplicationModelProvider>());
            //services.TryAddEnumerable(ServiceDescriptor.Transient<IActionDescriptorProvider, ControllerActionDescriptorProvider>());
            //services.TryAddSingleton<IActionDescriptorCollectionProvider, DefaultActionDescriptorCollectionProvider>();
            //services.TryAddSingleton<IActionSelector, ActionSelector>();
            //services.TryAddSingleton<ActionConstraintCache>();
            //services.TryAddEnumerable(ServiceDescriptor.Transient<IActionConstraintProvider, DefaultActionConstraintProvider>());
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, ActionConstraintMatcherPolicy>());
            //services.TryAddSingleton<IControllerFactory, DefaultControllerFactory>();
            //services.TryAddTransient<IControllerActivator, DefaultControllerActivator>();
            //services.TryAddSingleton<IControllerFactoryProvider, ControllerFactoryProvider>();
            //services.TryAddSingleton<IControllerActivatorProvider, ControllerActivatorProvider>();
            //services.TryAddEnumerable(ServiceDescriptor.Transient<IControllerPropertyActivator, DefaultControllerPropertyActivator>());
            //services.TryAddSingleton<IActionInvokerFactory, ActionInvokerFactory>();
            //services.TryAddEnumerable(ServiceDescriptor.Transient<IActionInvokerProvider, ControllerActionInvokerProvider>());
            //services.TryAddSingleton<ControllerActionInvokerCache>();
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<IFilterProvider, DefaultFilterProvider>());
            //services.TryAddSingleton<IActionResultTypeMapper, ActionResultTypeMapper>();
            //services.TryAddTransient<RequestSizeLimitFilter>();
            //services.TryAddTransient<DisableRequestSizeLimitFilter>();
            //services.TryAddTransient<RequestFormLimitsFilter>();
            //services.TryAddSingleton<IModelMetadataProvider, DefaultModelMetadataProvider>();
            //services.TryAdd(ServiceDescriptor.Transient((Func<IServiceProvider, ICompositeMetadataDetailsProvider>)((IServiceProvider s) => new DefaultCompositeMetadataDetailsProvider(s.GetRequiredService<IOptions<MvcOptions>>().Value.ModelMetadataDetailsProviders))));
            //services.TryAddSingleton<IModelBinderFactory, ModelBinderFactory>();
            //services.TryAddSingleton((Func<IServiceProvider, IObjectModelValidator>)delegate (IServiceProvider s)
            //{
            //    MvcOptions value = s.GetRequiredService<IOptions<MvcOptions>>().Value;
            //    return new DefaultObjectValidator(s.GetRequiredService<IModelMetadataProvider>(), value.ModelValidatorProviders, value);
            //});
            //services.TryAddSingleton<ClientValidatorCache>();
            //services.TryAddSingleton<ParameterBinder>();
            //services.TryAddSingleton<MvcMarkerService, MvcMarkerService>();
            //services.TryAddSingleton<ITypeActivatorCache, TypeActivatorCache>();
            //services.TryAddSingleton<IUrlHelperFactory, UrlHelperFactory>();
            //services.TryAddSingleton<IHttpRequestStreamReaderFactory, MemoryPoolHttpRequestStreamReaderFactory>();
            //services.TryAddSingleton<IHttpResponseStreamWriterFactory, MemoryPoolHttpResponseStreamWriterFactory>();
            //services.TryAddSingleton(ArrayPool<byte>.Shared);
            //services.TryAddSingleton(ArrayPool<char>.Shared);
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
            //services.TryAddSingleton<MvcRouteHandler>();
            //services.TryAddTransient<MvcAttributeRouteHandler>();
            //services.TryAddSingleton<ControllerActionEndpointDataSourceFactory>();
            //services.TryAddSingleton<OrderedEndpointsSequenceProviderCache>();
            //services.TryAddSingleton<ControllerActionEndpointDataSourceIdProvider>();
            //services.TryAddSingleton<ActionEndpointFactory>();
            //services.TryAddSingleton<DynamicControllerEndpointSelectorCache>();
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, DynamicControllerEndpointMatcherPolicy>());
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<IRequestDelegateFactory, ControllerRequestDelegateFactory>());
            //services.TryAddSingleton<MiddlewareFilterConfigurationProvider>();
            //services.TryAddSingleton<MiddlewareFilterBuilder>();
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<IStartupFilter, MiddlewareFilterBuilderStartupFilter>());
            //services.TryAddSingleton<ProblemDetailsFactory, DefaultProblemDetailsFactory>();
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<IProblemDetailsWriter, DefaultApiProblemDetailsWriter>());
        }

        private static void ConfigureDefaultServices(IServiceCollection services)
        {
            //services.AddRouting();
        }
    }

}
