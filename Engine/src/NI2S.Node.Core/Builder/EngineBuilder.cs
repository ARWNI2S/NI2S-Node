using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Internal;
using NI2S.Node.Dummy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NI2S.Node.Builder
{
    /// <summary>
    /// Default implementation for <see cref="IEngineBuilder"/>.
    /// </summary>
    public class EngineBuilder : IEngineBuilder
    {
        private const string ServerFeaturesKey = "server.Features";
        private const string EngineServicesKey = "application.Services";

        private readonly List<Func<RequestDelegate, RequestDelegate>> _components = new();

        /// <summary>
        /// Initializes a new instance of <see cref="EngineBuilder"/>.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> for application services.</param>
        public EngineBuilder(IServiceProvider serviceProvider) : this(serviceProvider, new FeatureCollection())
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="EngineBuilder"/>.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> for application services.</param>
        /// <param name="server">The server instance that hosts the application.</param>
        public EngineBuilder(IServiceProvider serviceProvider, object server)
        {
            Properties = new Dictionary<string, object>(StringComparer.Ordinal);
            EngineServices = serviceProvider;

            SetProperty(ServerFeaturesKey, server);
        }

        private EngineBuilder(EngineBuilder builder)
        {
            Properties = new CopyOnWriteDictionary<string, object>(builder.Properties, StringComparer.Ordinal);
        }

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> for application services.
        /// </summary>
        public IServiceProvider EngineServices
        {
            get
            {
                return GetProperty<IServiceProvider>(EngineServicesKey)!;
            }
            set
            {
                SetProperty(EngineServicesKey, value);
            }
        }

        /// <summary>
        /// Gets the <see cref="IFeatureCollection"/> for server features.
        /// </summary>
        /// <remarks>
        /// An empty collection is returned if a server wasn't specified for the application builder.
        /// </remarks>
        public IFeatureCollection ServerFeatures
        {
            get
            {
                return GetProperty<IFeatureCollection>(ServerFeaturesKey)!;
            }
        }

        /// <summary>
        /// Gets a set of properties for <see cref="EngineBuilder"/>.
        /// </summary>
        public IDictionary<string, object> Properties { get; }

        private T GetProperty<T>(string key)
        {
            return Properties.TryGetValue(key, out var value) ? (T)value : default;
        }

        private void SetProperty<T>(string key, T value)
        {
            Properties[key] = value;
        }

        /// <summary>
        /// Adds the middleware to the application request pipeline.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns>An instance of <see cref="IEngineBuilder"/> after the operation has completed.</returns>
        public IEngineBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            _components.Add(middleware);
            return this;
        }

        /// <summary>
        /// Creates a copy of this application builder.
        /// <para>
        /// The created clone has the same properties as the current instance, but does not copy
        /// the request pipeline.
        /// </para>
        /// </summary>
        /// <returns>The cloned instance.</returns>
        public IEngineBuilder New()
        {
            return new EngineBuilder(this);
        }

        /// <summary>
        /// Produces a <see cref="RequestDelegate"/> that executes added middlewares.
        /// </summary>
        /// <returns>The <see cref="RequestDelegate"/>.</returns>
        public RequestDelegate Build()
        {
            RequestDelegate app = context =>
            {
                // If we reach the end of the pipeline, but we have an endpoint, then something unexpected has happened.
                // This could happen if user code sets an endpoint, but they forgot to add the UseEndpoint middleware.
                var endpoint = context.GetEndpoint();
                var endpointRequestDelegate = endpoint?.RequestDelegate;
                if (endpointRequestDelegate != null)
                {
                    var message =
                        $"The request reached the end of the pipeline without executing the endpoint: '{endpoint!.DisplayName}'. " +
                        $"Please register the EndpointMiddleware using '{nameof(IEngineBuilder)}.UseEndpoints(...)' if using " +
                        $"routing.";
                    throw new InvalidOperationException(message);
                }

                //context.Response.StatusCode = StatusCodes.Status404NotFound;
                return Task.CompletedTask;
            };

            for (var c = _components.Count - 1; c >= 0; c--)
            {
                app = _components[c](app);
            }

            return app;
        }
    }
}
