using ARWNI2S.Engine.Core.Builder;
using ARWNI2S.Engine.Extensibility;
using ARWNI2S.Engine.Hosting;
using ARWNI2S.Engine.Parts;
using ARWNI2S.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Buffers;

namespace ARWNI2S.Engine.Core
{
    public static class NI2SCoreServiceCollectionExtensions
    {
        public static INiisCoreBuilder AddNI2SCore(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services, "services");
            INiisHostEnvironment serviceFromCollection = GetServiceFromCollection<INiisHostEnvironment>(services);
            EnginePartManager applicationPartManager = GetEnginePartManager(services, serviceFromCollection);
            services.TryAddSingleton(applicationPartManager);
            ConfigureDefaultProviders(applicationPartManager);
            ConfigureDefaultServices(services);
            AddNI2SCoreServices(services);
            return new NI2SCoreBuilder(services, applicationPartManager);
        }

        private static void ConfigureDefaultProviders(EnginePartManager manager)
        {
            if (!manager.ServiceProviders.OfType<EntityServiceProvider>().Any())
            {
                manager.ServiceProviders.Add(new EntityServiceProvider());
            }
        }

        private static EnginePartManager GetEnginePartManager(IServiceCollection services, INiisHostEnvironment environment)
        {
            EnginePartManager applicationPartManager = GetServiceFromCollection<EnginePartManager>(services);
            if (applicationPartManager == null)
            {
                applicationPartManager = new EnginePartManager();
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
            return (T)(services.LastOrDefault((d) => d.ServiceType == typeof(T))?.ImplementationInstance);
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
            ////services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, MvcCoreMvcOptionsSetup>());
            ////services.TryAddEnumerable(ServiceDescriptor.Transient<IPostConfigureOptions<MvcOptions>, MvcCoreMvcOptionsSetup>());
            ////services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<AiBehaviorOptions>, AiBehaviorOptionsSetup>());
            ////services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<RouteOptions>, MvcCoreRouteOptionsSetup>());
            //services.TryAddSingleton<EngineModelFactory>();
            //services.TryAddEnumerable(ServiceDescriptor.Transient<IEngineModelProvider, DefaultEngineModelProvider>());
            //services.TryAddEnumerable(ServiceDescriptor.Transient<IEngineModelProvider, AiBehaviorEngineModelProvider>());
            //services.TryAddEnumerable(ServiceDescriptor.Transient<IActionDescriptorProvider, ActorActionDescriptorProvider>());
            //services.TryAddSingleton<IActionDescriptorCollectionProvider, DefaultActionDescriptorCollectionProvider>();
            //services.TryAddSingleton<IActionSelector, ActionSelector>();
            //services.TryAddSingleton<ActionConstraintCache>();
            //services.TryAddEnumerable(ServiceDescriptor.Transient<IActionConstraintProvider, DefaultActionConstraintProvider>());
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, ActionConstraintMatcherPolicy>());
            //services.TryAddSingleton<IActorFactory, DefaultActorFactory>();
            //services.TryAddTransient<IActorActivator, DefaultActorActivator>();
            //services.TryAddSingleton<IActorFactoryProvider, ActorFactoryProvider>();
            //services.TryAddSingleton<IActorActivatorProvider, ActorActivatorProvider>();
            //services.TryAddEnumerable(ServiceDescriptor.Transient<IActorStateActivator, DefaultActorStateActivator>());
            //services.TryAddSingleton<IActionInvokerFactory, ActionInvokerFactory>();
            //services.TryAddEnumerable(ServiceDescriptor.Transient<IActionInvokerProvider, ActorActionInvokerProvider>());
            //services.TryAddSingleton<ActorActionInvokerCache>();
            ////services.TryAddEnumerable(ServiceDescriptor.Singleton<IFilterProvider, DefaultFilterProvider>());
            //services.TryAddSingleton<IActionResultTypeMapper, ActionResultTypeMapper>();

            ////services.TryAddTransient<RequestSizeLimitFilter>();
            ////services.TryAddTransient<DisableRequestSizeLimitFilter>();Update
            ////services.TryAddTransient<RequestFormLimitsFilter>();
            //services.TryAddSingleton<IModelMetadataProvider, DefaultModelMetadataProvider>();
            //services.TryAdd(ServiceDescriptor.Transient((Func<IServiceProvider, ICompositeMetadataDetailsProvider>)((s) => new DefaultCompositeMetadataDetailsProvider(s.GetRequiredService<NodeSettings>().Get<EngineConfig>().ModelMetadataDetailsProviders))));
            //services.TryAddSingleton<IModelBinderFactory, ModelBinderFactory>();
            //services.TryAddSingleton((Func<IServiceProvider, IObjectModelValidator>)delegate (IServiceProvider s)
            //{
            //    var config = Singleton<NodeSettings>.Instance.Get<EngineConfig>();
            //    return new DefaultObjectValidator(s.GetRequiredService<IModelMetadataProvider>(), config.ModelValidatorProviders, config);
            //});
            ////services.TryAddSingleton<ClientValidatorCache>();
            //services.TryAddSingleton<ParameterBinder>();
            //services.TryAddSingleton<NI2SMarkerService, NI2SMarkerService>();
            //services.TryAddSingleton<ITypeActivatorCache, TypeActivatorCache>();

            ////services.TryAddSingleton<IUrlHelperFactory, UrlHelperFactory>();
            ////services.TryAddSingleton<IHttpRequestStreamReaderFactory, MemoryPoolHttpRequestStreamReaderFactory>();
            ////services.TryAddSingleton<IHttpResponseStreamWriterFactory, MemoryPoolHttpResponseStreamWriterFactory>();
            services.TryAddSingleton(ArrayPool<byte>.Shared);
            services.TryAddSingleton(ArrayPool<char>.Shared);
            ////services.TryAddSingleton<OutputFormatterSelector, DefaultOutputFormatterSelector>();
            ////services.TryAddSingleton<IActionResultExecutor<ObjectResult>, ObjectResultExecutor>();
            ////services.TryAddSingleton<IActionResultExecutor<PhysicalFileResult>, PhysicalFileResultExecutor>();
            ////services.TryAddSingleton<IActionResultExecutor<VirtualFileResult>, VirtualFileResultExecutor>();
            ////services.TryAddSingleton<IActionResultExecutor<FileStreamResult>, FileStreamResultExecutor>();
            ////services.TryAddSingleton<IActionResultExecutor<FileContentResult>, FileContentResultExecutor>();
            ////services.TryAddSingleton<IActionResultExecutor<RedirectResult>, RedirectResultExecutor>();
            ////services.TryAddSingleton<IActionResultExecutor<LocalRedirectResult>, LocalRedirectResultExecutor>();
            ////services.TryAddSingleton<IActionResultExecutor<RedirectToActionResult>, RedirectToActionResultExecutor>();
            ////services.TryAddSingleton<IActionResultExecutor<RedirectToRouteResult>, RedirectToRouteResultExecutor>();
            ////services.TryAddSingleton<IActionResultExecutor<RedirectToPageResult>, RedirectToPageResultExecutor>();
            ////services.TryAddSingleton<IActionResultExecutor<ContentResult>, ContentResultExecutor>();
            ////services.TryAddSingleton<IActionResultExecutor<JsonResult>, SystemTextJsonResultExecutor>();
            //services.TryAddSingleton<IRemoteErrorFactory, ProblemDetailsRemoteErrorFactory>();
            ////services.TryAddSingleton<MvcRouteHandler>();
            ////services.TryAddTransient<MvcAttributeRouteHandler>();
            //services.TryAddSingleton<ActorActionUpdateDataSourceFactory>();
            //services.TryAddSingleton<OrderedUpdatesSequenceProviderCache>();
            //services.TryAddSingleton<ActorActionUpdateDataSourceIdProvider>();
            //services.TryAddSingleton<ActionUpdateFactory>();
            //services.TryAddSingleton<DynamicActorUpdateSelectorCache>();
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, DynamicActorUpdateMatcherPolicy>());
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<IUpdateDelegateFactory, ActorUpdateDelegateFactory>());

            ////services.TryAddSingleton<MiddlewareFilterConfigurationProvider>();
            ////services.TryAddSingleton<MiddlewareFilterBuilder>();
            ////services.TryAddEnumerable(ServiceDescriptor.Singleton<IStartupFilter, MiddlewareFilterBuilderStartupFilter>());
            //services.TryAddSingleton<ProblemDetailsFactory, DefaultProblemDetailsFactory>();
            ////services.TryAddEnumerable(ServiceDescriptor.Singleton<IProblemDetailsWriter, DefaultApiProblemDetailsWriter>());
        }

        private static void ConfigureDefaultServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddMemoryCache();
            services.AddDistributedCache();
        }
    }
}
