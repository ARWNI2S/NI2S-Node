using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Configuration;
using System;

namespace NI2S.Node.Hosting.Builder
{
    /// <summary>
    /// Contains extension methods to <see cref="IServiceCollection"/>.
    /// </summary>
    public static class DummyServiceCollectionExtensions
    {
        /// <summary>
        /// Adds services required for routing requests.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddRouting(this IServiceCollection services)
        {
            services.AddRoutingCore();
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<RouteOptions>, RegexInlineRouteConstraintSetup>());
            return services;
        }

        /// <summary>
        /// Adds services required for routing requests. This is similar to
        /// <see cref="AddRouting(IServiceCollection)" /> except that it
        /// excludes certain options that can be opted in separately, if needed.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddRoutingCore(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            //services.TryAddTransient<IInlineConstraintResolver, DefaultInlineConstraintResolver>();
            //services.TryAddTransient<ObjectPoolProvider, DefaultObjectPoolProvider>();
            //services.TryAddSingleton<ObjectPool<UriBuildingContext>>(s =>
            //{
            //    var provider = s.GetRequiredService<ObjectPoolProvider>();
            //    return provider.Create<UriBuildingContext>(new UriBuilderContextPooledObjectPolicy());
            //});

            //// The TreeRouteBuilder is a builder for creating routes, it should stay transient because it's
            //// stateful.
            //services.TryAdd(ServiceDescriptor.Transient<TreeRouteBuilder>(s =>
            //{
            //    var loggerFactory = s.GetRequiredService<ILoggerFactory>();
            //    var objectPool = s.GetRequiredService<ObjectPool<UriBuildingContext>>();
            //    var constraintResolver = s.GetRequiredService<IInlineConstraintResolver>();
            //    return new TreeRouteBuilder(loggerFactory, objectPool, constraintResolver);
            //}));

            //services.TryAddSingleton(typeof(RoutingMarkerService));

            //// Setup global collection of endpoint data sources
            //var dataSources = new ObservableCollection<EndpointDataSource>();
            //services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<RouteOptions>, ConfigureRouteOptions>(
            //    serviceProvider => new ConfigureRouteOptions(dataSources)));

            //// Allow global access to the list of endpoints.
            //services.TryAddSingleton<EndpointDataSource>(s =>
            //{
            //    // Call internal ctor and pass global collection
            //    return new CompositeEndpointDataSource(dataSources);
            //});

            ////
            //// Default matcher implementation
            ////
            //services.TryAddSingleton<ParameterPolicyFactory, DefaultParameterPolicyFactory>();
            //services.TryAddSingleton<MatcherFactory, DfaMatcherFactory>();
            //services.TryAddTransient<DfaMatcherBuilder>();
            //services.TryAddSingleton<DfaGraphWriter>();
            //services.TryAddTransient<DataSourceDependentMatcher.Lifetime>();
            //services.TryAddSingleton<EndpointMetadataComparer>(services =>
            //{
            //    // This has no public constructor.
            //    return new EndpointMetadataComparer(services);
            //});

            //// Link generation related services
            //services.TryAddSingleton<LinkGenerator, DefaultLinkGenerator>();
            //services.TryAddSingleton<IEndpointAddressScheme<string>, EndpointNameAddressScheme>();
            //services.TryAddSingleton<IEndpointAddressScheme<RouteValuesAddress>, RouteValuesAddressScheme>();
            //services.TryAddSingleton<LinkParser, DefaultLinkParser>();

            ////
            //// Endpoint Selection
            ////
            //services.TryAddSingleton<EndpointSelector, DefaultEndpointSelector>();
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, HttpMethodMatcherPolicy>());
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, HostMatcherPolicy>());
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, AcceptsMatcherPolicy>());

            ////
            //// Misc infrastructure
            ////
            //services.TryAddSingleton<TemplateBinderFactory, DefaultTemplateBinderFactory>();
            //services.TryAddSingleton<RoutePatternTransformer, DefaultRoutePatternTransformer>();

            //// Set RouteHandlerOptions.ThrowOnBadRequest in development
            //services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<RouteHandlerOptions>, ConfigureRouteHandlerOptions>());

            return services;
        }

        /// <summary>
        /// Adds services required for routing requests.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="configureOptions">The routing options to configure the middleware with.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddRouting(
            this IServiceCollection services,
            Action<DummyOptions> configureOptions)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configureOptions);

            services.Configure(configureOptions);
            services.AddRouting();

            return services;
        }
    }
}
