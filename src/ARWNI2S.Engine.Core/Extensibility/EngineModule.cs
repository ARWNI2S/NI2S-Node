﻿using ARWNI2S.Engine.Builder;
using ARWNI2S.Engine.Lifecycle;
using ARWNI2S.Extensibility;
using ARWNI2S.Lifecycle;
using Microsoft.Extensions.DependencyInjection;
using Orleans;

namespace ARWNI2S.Engine.Extensibility
{
    /// <summary>
    /// Represents a module that participates in the engine lifecycle
    /// </summary>
    public abstract class EngineModule : ModuleBase, IEngineModule
    {
        /// <summary>
        /// Gets the order of the module in the lifecycle.
        /// </summary>
        public override int Order => NI2SLifecycleStage.RuntimeInitialize;

        /// <summary>
        /// Gets the module system name.
        /// </summary>
        public override string SystemName { get => GetType().Name.ToModuleName(); set => throw new NotImplementedException(); }
        /// <summary>
        /// Gets the module friendly name.
        /// </summary>
        public override string FriendlyName { get => GetType().Name.ToFriendlyModuleName(); set => throw new NotImplementedException(); }

        /// <summary>
        /// Configure the engine to use the module
        /// </summary>
        /// <param name="engineBuilder">The engine builder</param>
        public override void ConfigureEngine(IEngineBuilder engineBuilder)
        {
            engineBuilder.EngineServices.GetRequiredService<IEngineModuleManager>().Register(this);
            Participate(engineBuilder.EngineServices.GetRequiredService<IEngineLifecycleSubject>());
        }

        /// <summary>
        /// Participate in the engine lifecycle
        /// </summary>
        /// <param name="lifecycle">The lifecycle to participate in</param>
        public virtual void Participate(IEngineLifecycle lifecycle)
        {
            lifecycle.Subscribe(SystemName, Order, ct => OnStart(ct), ct => OnStop(ct));
        }

        /// <summary>
        /// Start the module
        /// </summary>
        /// <param name="token">Cancellation token</param>
        /// <returns>A task</returns>
        protected virtual Task OnStart(CancellationToken token)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Stop the module
        /// </summary>
        /// <param name="token">Cancellation token</param>
        /// <returns>A task</returns>
        protected virtual Task OnStop(CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }

    internal abstract class FrameworkModule : EngineModule
    {
        public override void ConfigureEngine(IEngineBuilder engineBuilder)
        {
            engineBuilder.EngineServices.GetRequiredService<IModuleManager>().Register(this);
            Participate(engineBuilder.EngineServices.GetRequiredService<IEngineLifecycleSubject>());
        }
    }
}