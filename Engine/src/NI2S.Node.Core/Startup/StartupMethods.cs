// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Hosting.Builder;
using System;
using System.Diagnostics;

namespace NI2S.Node
{
    internal sealed class StartupMethods
    {
        public StartupMethods(object instance, Action<INodeEngineBuilder> configure, Func<IServiceCollection, IServiceProvider> configureServices)
        {
            Debug.Assert(configure != null);
            Debug.Assert(configureServices != null);

            StartupInstance = instance;
            ConfigureDelegate = configure;
            ConfigureServicesDelegate = configureServices;
        }

        public object StartupInstance { get; }
        public Func<IServiceCollection, IServiceProvider> ConfigureServicesDelegate { get; }
        public Action<INodeEngineBuilder> ConfigureDelegate { get; }
    }
}