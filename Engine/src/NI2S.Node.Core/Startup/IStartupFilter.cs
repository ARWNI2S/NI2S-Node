// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using NI2S.Node.Hosting.Builder;
using System;

namespace NI2S.Node
{
    /// <summary>
    /// Provides an interface for extending the middleware pipeline with new
    /// Configure methods. Can be used to add defaults to the beginning or
    /// end of the pipeline without having to make the app author explicitly
    /// register middleware.
    /// </summary>
    public interface IStartupFilter
    {
        /// <summary>
        /// Extends the provided <paramref name="next"/> and returns an <see cref="Action"/> of the same type.
        /// </summary>
        /// <param name="next">The Configure method to extend.</param>
        /// <returns>A modified <see cref="Action"/>.</returns>
        Action<INodeEngineBuilder> Configure(Action<INodeEngineBuilder> next);
    }
}