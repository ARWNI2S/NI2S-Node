using NI2S.Node.Dummy;
using System;

namespace NI2S.Node.Hosting.Builder
{
    /// <summary>
    /// Extension methods for adding the <see cref="RouterMiddleware"/> middleware to an <see cref="INodeBuilder"/>.
    /// </summary>
    public static class DummyBuilderExtensions
    {
        /// <summary>
        /// Adds a <see cref="RouterMiddleware"/> middleware to the specified <see cref="INodeBuilder"/> with the specified <see cref="IDummyRouter"/>.
        /// </summary>
        /// <param name="builder">The <see cref="INodeBuilder"/> to add the middleware to.</param>
        /// <param name="router">The <see cref="IDummyRouter"/> to use for routing requests.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static INodeBuilder UseRouter(this INodeBuilder builder, IDummyRouter router)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(router);

            //TODO NODE PORTS
            //if (builder.ApplicationServices.GetService(typeof(RoutingMarkerService)) == null)
            //{
            //    throw new InvalidOperationException(Resources.FormatUnableToFindServices(
            //        nameof(IServiceCollection),
            //        nameof(RoutingServiceCollectionExtensions.AddRouting),
            //        "ConfigureServices(...)"));
            //}

            //return builder.UseMiddleware<RouterMiddleware>(router);
            return builder;
        }

        /// <summary>
        /// Adds a <see cref="RouterMiddleware"/> middleware to the specified <see cref="INodeBuilder"/>
        /// with the <see cref="IDummyRouter"/> built from configured <see cref="IDummyRouteBuilder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="INodeBuilder"/> to add the middleware to.</param>
        /// <param name="action">An <see cref="Action{IRouteBuilder}"/> to configure the provided <see cref="IDummyRouteBuilder"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static INodeBuilder UseRouter(this INodeBuilder builder, Action<IDummyRouteBuilder> action)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(action);

            //TODO NODE PORTS
            //if (builder.ApplicationServices.GetService(typeof(RoutingMarkerService)) == null)
            //{
            //    throw new InvalidOperationException(Resources.FormatUnableToFindServices(
            //        nameof(IServiceCollection),
            //        nameof(RoutingServiceCollectionExtensions.AddRouting),
            //        "ConfigureServices(...)"));
            //}

            //var routeBuilder = new RouteBuilder(builder);
            //action(routeBuilder);

            //return builder.UseRouter(routeBuilder.Build());
            return builder;
        }
    }
}
