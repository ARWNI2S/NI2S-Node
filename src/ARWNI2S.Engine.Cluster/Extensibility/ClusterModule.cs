using ARWNI2S.Cluster.Lifecycle;
using ARWNI2S.Cluster.Networking;
using ARWNI2S.Engine.Builder;
using ARWNI2S.Engine.Data;
using ARWNI2S.Engine.Extensibility;
using ARWNI2S.Extensibility;
using Microsoft.Extensions.DependencyInjection;
using Orleans;

namespace ARWNI2S.Cluster.Extensibility
{
    /// <summary>
    /// Represents a module that participates in the engine lifecycle
    /// </summary>
    public abstract class ClusterModule : ModuleBase, IClusterModule
    {
        /// <summary>
        /// Gets the order of the module in the lifecycle.
        /// </summary>
        public override int Order => ClusterLifecycleStage.ClusterServices;

        /// <summary>
        /// Gets the module system name.
        /// </summary>
        public override string SystemName { get => GetType().Name.ToModuleName(); set => throw new NotImplementedException(); }
        /// <summary>
        /// Gets the module friendly name.
        /// </summary>
        public override string DisplayName { get => GetType().Name.ToFriendlyModuleName(); set => throw new NotImplementedException(); }

        public override IList<string> ModuleDependencies => [nameof(NI2SDataModule).ToModuleName(),
                                                             nameof(NI2SNetworkModule).ToModuleName()];

        /// <summary>
        /// Configure the engine to use the module
        /// </summary>
        /// <param name="engineBuilder">The engine builder</param>
        public override void ConfigureEngine(IEngineBuilder engineBuilder)
        {
            engineBuilder.EngineServices.GetRequiredService<IModuleManager>().Register(this);
            Participate(engineBuilder.EngineServices.GetRequiredService<IClusterNodeLifecycle>());
        }

        /// <summary>
        /// Participate in the engine lifecycle
        /// </summary>
        /// <param name="lifecycle">The lifecycle to participate in</param>
        public virtual void Participate(IClusterNodeLifecycle lifecycle)
        {
            lifecycle.Subscribe(SystemName, Order, ct => OnStart(ct), ct => OnStop(ct));
        }

        private Task OnStart(CancellationToken token)
        {
            return Task.Run(() => OnModuleStart(), token);
        }

        private Task OnStop(CancellationToken token)
        {
            return Task.Run(() => OnModuleStop(), token);
        }

        /// <summary>
        /// Start the module
        /// </summary>
        /// <returns>A task</returns>
        protected virtual void OnModuleStart() { /*Do nothing.*/ }

        /// <summary>
        /// Stop the module
        /// </summary>
        /// <returns>A task</returns>
        protected virtual void OnModuleStop() { /*Do nothing.*/ }
    }

}
