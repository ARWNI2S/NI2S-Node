﻿using NI2S.Node.Dummy;
using System;

namespace NI2S.Node.Routing
{
    /// <summary>
    /// A context object for <see cref="IRouter.RouteAsync(RouteContext)"/>.
    /// </summary>
    public class RouteContext
    {
        private RouteData _routeData;

        /// <summary>
        /// Creates a new instance of <see cref="RouteContext"/> for the provided <paramref name="dummyContext"/>.
        /// </summary>
        /// <param name="dummyContext">The <see cref="Dummy.DummyContext"/> associated with the current request.</param>
        public RouteContext(DummyContext dummyContext)
        {
            DummyContext = dummyContext ?? throw new ArgumentNullException(nameof(dummyContext));

            RouteData = new RouteData();
        }

        /// <summary>
        /// Gets or sets the handler for the request. An <see cref="IRouter"/> should set <see cref="Handler"/>
        /// when it matches.
        /// </summary>
        public RequestDelegate Handler { get; set; }

        /// <summary>
        /// Gets the <see cref="Dummy.DummyContext"/> associated with the current request.
        /// </summary>
        public DummyContext DummyContext { get; }

        /// <summary>
        /// Gets or sets the <see cref="Routing.RouteData"/> associated with the current context.
        /// </summary>
        public RouteData RouteData
        {
            get
            {
                return _routeData;
            }
            set
            {
                _routeData = value ?? throw new ArgumentNullException(nameof(RouteData));
            }
        }
    }
}