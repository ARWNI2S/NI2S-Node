﻿using NI2S.Node.Dummy;
using System;
using System.Collections.Generic;

namespace NI2S.Node.Builder
{
    /// <summary>
    /// A base class for building an new <see cref="Endpoint"/>.
    /// </summary>
    public abstract class EndpointBuilder
    {
        private List<Func<EndpointFilterFactoryContext, EndpointFilterDelegate, EndpointFilterDelegate>> _filterFactories;

        /// <summary>
        /// Gets the list of filters that apply to this endpoint.
        /// </summary>
        public IList<Func<EndpointFilterFactoryContext, EndpointFilterDelegate, EndpointFilterDelegate>> FilterFactories => _filterFactories ??= new();

        /// <summary>
        /// Gets or sets the delegate used to process requests for the endpoint.
        /// </summary>
        public RequestDelegate RequestDelegate { get; set; }

        /// <summary>
        /// Gets or sets the informational display name of this endpoint.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets the collection of metadata associated with this endpoint.
        /// </summary>
        public IList<object> Metadata { get; } = new List<object>();

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> associated with the endpoint.
        /// </summary>
        public IServiceProvider EngineServices { get; init; } = EmptyServiceProvider.Instance;

        /// <summary>
        /// Creates an instance of <see cref="Endpoint"/> from the <see cref="EndpointBuilder"/>.
        /// </summary>
        /// <returns>The created <see cref="Endpoint"/>.</returns>
        public abstract Endpoint Build();

        private sealed class EmptyServiceProvider : IServiceProvider
        {
            public static EmptyServiceProvider Instance { get; } = new EmptyServiceProvider();
            public object GetService(Type serviceType) => null;
        }
    }
}
