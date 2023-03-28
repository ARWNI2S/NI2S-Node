using System;

namespace NI2S.Node.Engine.Middleware
{
    /// <summary>
    /// Provides methods to create middleware.
    /// </summary>
    public interface IDummyMiddlewareFactory
    {
        /// <summary>
        /// Creates a middleware instance for each request.
        /// </summary>
        /// <param name="middlewareType">The concrete <see cref="Type"/> of the <see cref="IDummyMiddleware"/>.</param>
        /// <returns>The <see cref="IDummyMiddleware"/> instance.</returns>
        IDummyMiddleware Create(Type middlewareType);

        /// <summary>
        /// Releases a <see cref="IDummyMiddleware"/> instance at the end of each request.
        /// </summary>
        /// <param name="middleware">The <see cref="IDummyMiddleware"/> instance to release.</param>
        void Release(IDummyMiddleware middleware);
    }
}
