﻿// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Engine;
using NI2S.Node.Hosting.Builder;
using System;
using System.Collections.Generic;

namespace NI2S.Node.Core.Infrastructure
{
    /// <summary>
    /// Classes implementing this interface can serve as a portal for the various services composing the NI2S engine. 
    /// Edit functionality, modules and implementations access most NI2S functionality through this interface.
    /// </summary>
    public interface IEngine
    {

        /// <summary>
        /// Service provider
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Engine modules
        /// </summary>
        IModuleCollection Modules { get; }

        /// <summary>
        /// Add and configure services.
        /// </summary>
        /// <param name="services">Collection of service descriptors.</param>
        /// <param name="configuration">Configuration of the engine.</param>
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);

        /// <summary>
        /// Configure event handler pipeline.
        /// </summary>
        /// <param name="engine">Builder for configuring a engine's event handler for specific simulation.</param>
        void ConfigureEventHandler(IEngineBuilder engine);

        /// <summary>
        /// Resolve dependency.
        /// </summary>
        /// <param name="scope">Scope.</param>
        /// <typeparam name="T">Type of resolved service.</typeparam>
        /// <returns>Resolved service.</returns>
        T Resolve<T>(IServiceScope scope = null) where T : class;

        /// <summary>
        /// Resolve dependency.
        /// </summary>
        /// <param name="type">Type of resolved service.</param>
        /// <param name="scope">Scope.</param>
        /// <returns>Resolved service.</returns>
        object Resolve(Type type, IServiceScope scope = null);

        /// <summary>
        /// Resolve dependencies.
        /// </summary>
        /// <typeparam name="T">Type of resolved services.</typeparam>
        /// <returns>Collection of resolved services.</returns>
        IEnumerable<T> ResolveAll<T>();

        /// <summary>
        /// Resolve unregistered service.
        /// </summary>
        /// <param name="type">Type of service.</param>
        /// <returns>Resolved service.</returns>
        object ResolveUnregistered(Type type);
    }
}