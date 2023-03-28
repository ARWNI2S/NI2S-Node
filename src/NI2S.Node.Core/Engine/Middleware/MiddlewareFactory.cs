using Microsoft.Extensions.DependencyInjection;
using System;

namespace NI2S.Node.Engine.Middleware
{
    /// <summary>
    /// Default implementation for <see cref="IDummyMiddlewareFactory"/>.
    /// </summary>
    public class MiddlewareFactory : IDummyMiddlewareFactory
    {
        // The default middleware factory is just an IServiceProvider proxy.
        // This should be registered as a scoped service so that the middleware instances
        // don't end up being singletons.
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of <see cref="MiddlewareFactory"/>.
        /// </summary>
        /// <param name="serviceProvider">The application services.</param>
        public MiddlewareFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc/>
        public IDummyMiddleware Create(Type middlewareType)
        {
            return _serviceProvider.GetRequiredService(middlewareType) as IDummyMiddleware;
        }

        /// <inheritdoc/>
        public void Release(IDummyMiddleware middleware)
        {
            // The container owns the lifetime of the service
        }
    }
}
