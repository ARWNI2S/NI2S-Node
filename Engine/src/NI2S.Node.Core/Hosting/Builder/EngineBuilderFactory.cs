// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.AspNetCore.Http.Features;
using NI2S.Node.Engine;
using System;

namespace NI2S.Node.Hosting.Builder
{
    internal class EngineBuilderFactory : IEngineBuilderFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initialize a new factory instance with an <see cref="IServiceProvider" />.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> used to resolve dependencies and initialize components.</param>
        public EngineBuilderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Create an <see cref="IEngineBuilder" /> builder given a <paramref name="engineModules" />.
        /// </summary>
        /// <param name="engineModules">An <see cref="IFeatureCollection"/> of HTTP features.</param>
        /// <returns>An <see cref="IEngineBuilder"/> configured with <paramref name="engineModules"/>.</returns>
        public IEngineBuilder CreateBuilder(IModuleCollection engineModules)
        {
            return new EngineBuilder(_serviceProvider, engineModules);
        }
    }
}