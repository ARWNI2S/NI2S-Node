using NI2S.Node.Builder;
using NI2S.Node.Dummy;
using System;

namespace NI2S.Node
{
    /// <summary>
    /// Extension methods for adding terminal middleware.
    /// </summary>
    public static class RunExtensions
    {
        /// <summary>
        /// Adds a terminal middleware delegate to the application's request pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IEngineBuilder"/> instance.</param>
        /// <param name="handler">A delegate that handles the request.</param>
        public static void Run(this IEngineBuilder app, RequestDelegate handler)
        {
            ArgumentNullException.ThrowIfNull(app);
            ArgumentNullException.ThrowIfNull(handler);

            app.Use(_ => handler);
        }
    }
}
