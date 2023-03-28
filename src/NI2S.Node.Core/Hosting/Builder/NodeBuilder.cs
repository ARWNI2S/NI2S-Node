using NI2S.Node.Hosting.Builder;
using NI2S.Node.Core.Internal;
using NI2S.Node.Engine.Modules;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NI2S.Node.Dummy;

namespace NI2S.Node.Hosting.Builder
{
    /// <summary>
    /// Default implementation for <see cref="INodeBuilder"/>.
    /// </summary>
    public class NodeBuilder : INodeBuilder
    {
        private const string ServerFeaturesKey = "server.Features";
        private const string ApplicationServicesKey = "application.Services";

        private readonly List<Func<MessageDelegate, MessageDelegate>> _components = new();

        /// <summary>
        /// Initializes a new instance of <see cref="NodeBuilder"/>.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> for application services.</param>
        public NodeBuilder(IServiceProvider serviceProvider) : this(serviceProvider, new DummyFeatureCollection())
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NodeBuilder"/>.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> for application services.</param>
        /// <param name="server">The server instance that hosts the application.</param>
        public NodeBuilder(IServiceProvider serviceProvider, object server)
        {
            Properties = new Dictionary<string, object>(StringComparer.Ordinal);
            ApplicationServices = serviceProvider;

            SetProperty(ServerFeaturesKey, server);
        }

        private NodeBuilder(NodeBuilder builder)
        {
            Properties = new CopyOnWriteDictionary<string, object>(builder.Properties, StringComparer.Ordinal);
        }

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> for application services.
        /// </summary>
        public IServiceProvider ApplicationServices
        {
            get
            {
                return GetProperty<IServiceProvider>(ApplicationServicesKey)!;
            }
            set
            {
                SetProperty<IServiceProvider>(ApplicationServicesKey, value);
            }
        }

        /// <summary>
        /// Gets the <see cref="IDummyFeatureCollection"/> for server features.
        /// </summary>
        /// <remarks>
        /// An empty collection is returned if a server wasn't specified for the application builder.
        /// </remarks>
        public IDummyFeatureCollection ServerFeatures
        {
            get
            {
                return GetProperty<IDummyFeatureCollection>(ServerFeaturesKey)!;
            }
        }

        /// <summary>
        /// Gets a set of properties for <see cref="NodeBuilder"/>.
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
        /// <returns>An instance of <see cref="INodeBuilder"/> after the operation has completed.</returns>
        public INodeBuilder Use(Func<MessageDelegate, MessageDelegate> middleware)
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
        public INodeBuilder New()
        {
            return new NodeBuilder(this);
        }

        /// <summary>
        /// Produces a <see cref="MessageDelegate"/> that executes added middlewares.
        /// </summary>
        /// <returns>The <see cref="MessageDelegate"/>.</returns>
        public MessageDelegate Build()
        {
            MessageDelegate app = context =>
            {
                // If we reach the end of the pipeline, but we have an endpoint, then something unexpected has happened.
                // This could happen if user code sets an endpoint, but they forgot to add the UseEndpoint middleware.
                var endpoint = context.GetEndpoint();
                var endpointMessageDelegate = endpoint?.MessageDelegate;
                if (endpointMessageDelegate != null)
                {
                    var message =
                        $"The request reached the end of the pipeline without executing the endpoint: '{endpoint!.DisplayName}'. " +
                        $"Please register the EndpointMiddleware using '{nameof(INodeBuilder)}.UseEndpoints(...)' if using " +
                        $"routing.";
                    throw new InvalidOperationException(message);
                }

                context.Response.StatusCode = DummyStatusCodes.Status404NotFound;
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