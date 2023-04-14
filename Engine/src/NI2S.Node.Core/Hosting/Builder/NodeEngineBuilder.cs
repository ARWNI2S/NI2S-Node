// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.Internal;
using NI2S.Node.Engine;
using System;
using System.Collections.Generic;

namespace NI2S.Node.Hosting.Builder
{
    /// <summary>
    /// Default implementation for <see cref="INodeEngineBuilder"/>.
    /// </summary>
    internal class NodeEngineBuilder : INodeEngineBuilder
    {
        private const string EngineModulesKey = "engine.Modules";
        private const string EngineServicesKey = "engine.Services";

        //private readonly List<Func<INodeEngine, INodeEngine>> _components = new();

        /// <summary>
        /// Initializes a new instance of <see cref="NodeEngineBuilder"/>.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> for application services.</param>
        public NodeEngineBuilder(IServiceProvider serviceProvider) : this(serviceProvider, new ModuleCollection())
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NodeEngineBuilder"/>.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> for application services.</param>
        /// <param name="server">The server instance that hosts the application.</param>
        public NodeEngineBuilder(IServiceProvider serviceProvider, object server)
        {
            Properties = new Dictionary<string, object>(StringComparer.Ordinal);
            EngineServices = serviceProvider;

            SetProperty(EngineModulesKey, server);
        }

        private NodeEngineBuilder(NodeEngineBuilder builder)
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
        /// Gets the <see cref="IModuleCollection"/> for server modules.
        /// </summary>
        /// <remarks>
        /// An empty collection is returned if a server wasn't specified for the application builder.
        /// </remarks>
        public IModuleCollection EngineModules
        {
            get
            {
                return GetProperty<IModuleCollection>(EngineModulesKey)!;
            }
        }

        /// <summary>
        /// Gets a set of properties for <see cref="NodeEngineBuilder"/>.
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
        /// <returns>An instance of <see cref="INodeEngineBuilder"/> after the operation has completed.</returns>
        //public IEngineBuilder Use(Func<INodeEngine, INodeEngine> middleware)
        //{
        //    _components.Add(middleware);
        //    return this;
        //}

        /// <summary>
        /// Creates a copy of this application builder.
        /// <para>
        /// The created clone has the same properties as the current instance, but does not copy
        /// the request pipeline.
        /// </para>
        /// </summary>
        /// <returns>The cloned instance.</returns>
        public INodeEngineBuilder New()
        {
            return new NodeEngineBuilder(this);
        }

        /// <summary>
        /// Produces a <see cref="INodeEngine"/> that executes added middlewares.
        /// </summary>
        /// <returns>The <see cref="INodeEngine"/>.</returns>
        public INodeEngine Build()
        {
            //TODO: BUILD NODE ENGINE
            //INodeEngine app = context =>
            //{
            //    // If we reach the end of the pipeline, but we have an endpoint, then something unexpected has happened.
            //    // This could happen if user code sets an endpoint, but they forgot to add the UseEndpoint middleware.
            //    var endpoint = context.GetEndpoint();
            //    var endpointMessageDelegate = endpoint?.INodeEngine;
            //    if (endpointMessageDelegate != null)
            //    {
            //        var message =
            //            $"The request reached the end of the pipeline without executing the endpoint: '{endpoint!.DisplayName}'. " +
            //            $"Please register the EndpointMiddleware using '{nameof(IEngineBuilder)}.UseEndpoints(...)' if using " +
            //            $"routing.";
            //        throw new InvalidOperationException(message);
            //    }

            //    context.Response.StatusCode = StatusCodes.Status404NotFound;
            //    return Task.CompletedTask;
            //};

            //for (var c = _components.Count - 1; c >= 0; c--)
            //{
            //    app = _components[c](app);
            //}

            //return app;
            return null;
        }
    }
}
