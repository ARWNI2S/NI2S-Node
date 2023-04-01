﻿using Microsoft.AspNetCore.Http.Features;

namespace NI2S.Node.Hosting.Builder
{
    /// <summary>
    /// Provides an interface for implementing a factory that produces <see cref="IApplicationBuilder"/> instances.
    /// </summary>
    public interface IApplicationBuilderFactory
    {
        /// <summary>
        /// Create an <see cref="IApplicationBuilder" /> builder given a <paramref name="serverFeatures" />
        /// </summary>
        /// <param name="serverFeatures">An <see cref="IFeatureCollection"/> of HTTP features.</param>
        /// <returns>An <see cref="IApplicationBuilder"/> configured with <paramref name="serverFeatures"/>.</returns>
        IApplicationBuilder CreateBuilder(IFeatureCollection serverFeatures);
    }
}
