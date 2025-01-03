using ARWNI2S.Cluster.Lifecycle;
using ARWNI2S.Engine;
using ARWNI2S.Engine.Data;
using ARWNI2S.Engine.Data.Migrations;
using ARWNI2S.Engine.Lifecycle;
using ARWNI2S.Engine.Plugins;
using ARWNI2S.Lifecycle;
using ARWNI2S.Node.Diagnostics;
using Microsoft.Extensions.Logging;
using Orleans;
using System.Diagnostics;
using System.Reflection;

namespace ARWNI2S.Node
{
    internal class EngineHost : INiisEngine, ILifecycleParticipant<IEngineLifecycle>
    {
        private readonly INiisEngine _engine;
        private readonly ILogger _logger;
        private readonly DiagnosticListener _diagnosticListener;
        private readonly ActivitySource _activitySource;
        private readonly DistributedContextPropagator _propagator;
        private readonly INiisContextFactory _contextFactory;
        private readonly object _log;
        private readonly HostingMetrics _hostingMetrics;

        public EngineHost(INiisEngine engine, ILogger logger, DiagnosticListener diagnosticListener, ActivitySource activitySource, DistributedContextPropagator propagator, INiisContextFactory contextFactory, object log, HostingMetrics hostingMetrics)
        {
            this._engine = engine;
            this._logger = logger;
            this._diagnosticListener = diagnosticListener;
            this._activitySource = activitySource;
            this._propagator = propagator;
            this._contextFactory = contextFactory;
            this._log = log;
            this._hostingMetrics = hostingMetrics;


            var context = EngineContext.Current;

            var engineLifecycle = context.Resolve<IEngineLifecycleSubject>();

            if (engineLifecycle != null)
            {
                Participate(engineLifecycle);

                // register all lifecycle participants
                IEnumerable<ILifecycleParticipant<IEngineLifecycle>> lifecycleParticipants = context.ResolveAll<ILifecycleParticipant<IEngineLifecycle>>();
                foreach (var participant in lifecycleParticipants)
                {
                    participant?.Participate(engineLifecycle);
                }

            }

            var clusterLifecycle = context.Resolve<IClusterNodeLifecycle>();
            if (clusterLifecycle != null)
            {
                // register all lifecycle participants
                IEnumerable<ILifecycleParticipant<IClusterNodeLifecycle>> lifecycleParticipants = context.ResolveAll<ILifecycleParticipant<IClusterNodeLifecycle>>();
                foreach (var participant in lifecycleParticipants)
                {
                    participant?.Participate(clusterLifecycle);
                }

            }

            //static void ValidateSystemConfiguration(IServiceProvider serviceProvider)
            //{
            //    var validators = serviceProvider.GetServices<IConfigurationValidator>();
            //    foreach (var validator in validators)
            //    {
            //        validator.ValidateConfiguration();
            //    }
            //}

        }

        public void Participate(IEngineLifecycle lifecycle)
        {
            lifecycle.Subscribe(nameof(EngineHost), NI2SLifecycleStage.First, token => OnFirstStage(token));
            lifecycle.Subscribe(nameof(EngineHost), NI2SLifecycleStage.EarlyInitialize, token => OnEarlyInitialize(token));
            lifecycle.Subscribe(nameof(EngineHost), NI2SLifecycleStage.PreCoreInitialize, token => OnPreCoreInitialize(token));
            lifecycle.Subscribe(nameof(EngineHost), NI2SLifecycleStage.CoreInitialize, token => OnCoreInitialize(token));
            lifecycle.Subscribe(nameof(EngineHost), NI2SLifecycleStage.PostCoreInitialize, token => OnPostCoreInitialize(token));
            lifecycle.Subscribe(nameof(EngineHost), NI2SLifecycleStage.RuntimeInitialize, token => OnRuntimeInitialize(token));
            lifecycle.Subscribe(nameof(EngineHost), NI2SLifecycleStage.RuntimeServices, token => OnRuntimeServices(token));
            lifecycle.Subscribe(nameof(EngineHost), NI2SLifecycleStage.RuntimeStorageServices, token => OnRuntimeStorageServices(token));
            lifecycle.Subscribe(nameof(EngineHost), NI2SLifecycleStage.RuntimeActorServices, token => OnRuntimeActorServices(token));
            lifecycle.Subscribe(nameof(EngineHost), NI2SLifecycleStage.AfterRuntimeActorServices, token => OnAfterRuntimeActorServices(token));
            lifecycle.Subscribe(nameof(EngineHost), NI2SLifecycleStage.SimulationServices, token => OnSimulationServices(token));
            lifecycle.Subscribe(nameof(EngineHost), NI2SLifecycleStage.BecomeActive, token => OnBecomeActive(token));
            lifecycle.Subscribe(nameof(EngineHost), NI2SLifecycleStage.Active, token => OnActive(token));
            lifecycle.Subscribe(nameof(EngineHost), NI2SLifecycleStage.Last, token => OnLastStage(token));
        }

        private async Task OnLastStage(CancellationToken token)
        {
            _logger.LogDebug("On Last Stage");

            var context = EngineContext.Current;

            //further actions are performed only when the database is installed
            if (DataSettingsManager.IsDatabaseInstalled())
            {
            }

            await Task.CompletedTask;
        }

        private async Task OnActive(CancellationToken token)
        {
            _logger.LogDebug("On Active");

            var context = EngineContext.Current;

            //further actions are performed only when the database is installed
            if (DataSettingsManager.IsDatabaseInstalled())
            {
                //log application start
                //await context.Resolve<ILogger>().InformationAsync("Application started");
            }

            await Task.CompletedTask;
        }

        private async Task OnBecomeActive(CancellationToken token)
        {
            _logger.LogDebug("On Become Active");

            var context = EngineContext.Current;

            //further actions are performed only when the database is installed
            if (DataSettingsManager.IsDatabaseInstalled())
            {
                //var taskScheduler = context.Resolve<ITaskScheduler>();
                //await taskScheduler.StartSchedulerAsync();
            }

            await Task.CompletedTask;
        }

        private async Task OnSimulationServices(CancellationToken token)
        {
            _logger.LogDebug("On Simulation Services");

            var context = EngineContext.Current;

            //further actions are performed only when the database is installed
            if (DataSettingsManager.IsDatabaseInstalled())
            {
            }

            await Task.CompletedTask;
        }

        private async Task OnAfterRuntimeActorServices(CancellationToken token)
        {
            _logger.LogDebug("On After Runtime Actor Services");

            var context = EngineContext.Current;

            //further actions are performed only when the database is installed
            if (DataSettingsManager.IsDatabaseInstalled())
            {
            }

            await Task.CompletedTask;
        }

        private async Task OnRuntimeActorServices(CancellationToken token)
        {
            _logger.LogDebug("On Runtime Actor Services");

            var context = EngineContext.Current;

            //further actions are performed only when the database is installed
            if (DataSettingsManager.IsDatabaseInstalled())
            {
            }

            await Task.CompletedTask;
        }

        private async Task OnRuntimeStorageServices(CancellationToken token)
        {
            _logger.LogDebug("On Runtime Storage Services");

            var context = EngineContext.Current;

            //further actions are performed only when the database is installed
            if (DataSettingsManager.IsDatabaseInstalled())
            {
            }

            await Task.CompletedTask;
        }

        private async Task OnRuntimeServices(CancellationToken token)
        {
            _logger.LogDebug("On Runtime Services");

            var context = EngineContext.Current;

            //further actions are performed only when the database is installed
            if (DataSettingsManager.IsDatabaseInstalled())
            {
            }

            await Task.CompletedTask;
        }

        private async Task OnRuntimeInitialize(CancellationToken token)
        {
            _logger.LogDebug("On Runtime Initialize");

            var context = EngineContext.Current;

            //further actions are performed only when the database is installed
            if (DataSettingsManager.IsDatabaseInstalled())
            {
                //var taskScheduler = context.Resolve<ITaskScheduler>();
                //await taskScheduler.InitializeAsync();
            }

            await Task.CompletedTask;
        }

        private async Task OnPostCoreInitialize(CancellationToken token)
        {
            _logger.LogDebug("On Post-Core Initialize");

            var context = EngineContext.Current;

            //further actions are performed only when the database is installed
            if (DataSettingsManager.IsDatabaseInstalled())
            {
            }

            await Task.CompletedTask;
        }

        private async Task OnCoreInitialize(CancellationToken token)
        {
            _logger.LogDebug("On Core Initialize");

            var context = EngineContext.Current;

            //further actions are performed only when the database is installed
            if (DataSettingsManager.IsDatabaseInstalled())
            {
                ////insert new ACL permission if exists
                //var permissionService = context.Resolve<IPermissionService>();
                //await permissionService.InsertPermissionsAsync();
            }

            await Task.CompletedTask;
        }

        private async Task OnPreCoreInitialize(CancellationToken token)
        {
            _logger.LogDebug("On Pre-Core Initialize");

            var context = EngineContext.Current;

            //further actions are performed only when the database is installed
            if (DataSettingsManager.IsDatabaseInstalled())
            {
                //install and update plugins
                var pluginService = context.Resolve<IPluginService>();
                await pluginService.InstallPluginsAsync();
                await pluginService.UpdatePluginsAsync();

                //update core and db
                var migrationManager = context.Resolve<IMigrationManager>();
                var assembly = Assembly.GetAssembly(typeof(EngineHost));
                migrationManager.ApplyUpMigrations(assembly, MigrationProcessType.Update);
                assembly = Assembly.GetAssembly(typeof(IMigrationManager));
                migrationManager.ApplyUpMigrations(assembly, MigrationProcessType.Update);
            }

            await Task.CompletedTask;
        }

        private async Task OnFirstStage(CancellationToken token)
        {
            _logger.LogDebug("On First Stage");

            var context = EngineContext.Current;

            //further actions are performed only when the database is installed
            if (DataSettingsManager.IsDatabaseInstalled())
            {
            }

            await Task.CompletedTask;
        }

        public async Task OnEarlyInitialize(CancellationToken token)
        {
            _logger.LogDebug("On Early Initialize");

            var context = EngineContext.Current;

            //further actions are performed only when the database is installed
            if (DataSettingsManager.IsDatabaseInstalled())
            {
            }

            await Task.CompletedTask;
        }



        public void Start()
        {
            var context = EngineContext.Current;

            //further actions are performed only when the database is installed
            if (DataSettingsManager.IsDatabaseInstalled())
            {



            }
        }
    }
}