using ARWNI2S.Lifecycle;
using ARWNI2S.Timers;
using Microsoft.Extensions.Logging;
using Orleans;
using System.Collections.Immutable;

namespace ARWNI2S.Engine.Lifecycle
{
    /// <summary>
    /// Decorator over lifecycle subject for silo.  Adds some logging and monitoring
    /// </summary>
    public class EngineLifecycleSubject : LifecycleSubject, IEngineLifecycleSubject
    {
        private static readonly ImmutableDictionary<int, string> StageNames = GetStageNames(typeof(NI2SLifecycleStage));
        private readonly List<MonitoredObserver> observers;
        private int highestCompletedStage;
        private int lowestStoppedStage;

        /// <inheritdoc />
        public int HighestCompletedStage => highestCompletedStage;

        /// <inheritdoc />
        public int LowestStoppedStage => lowestStoppedStage;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineLifecycleSubject"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public EngineLifecycleSubject(ILogger<EngineLifecycleSubject> logger) : base(logger)
        {
            observers = new List<MonitoredObserver>();
            highestCompletedStage = int.MinValue;
            lowestStoppedStage = int.MaxValue;
        }

        /// <inheritdoc />
        public override Task OnStart(CancellationToken cancellationToken = default)
        {
            foreach (var stage in observers.GroupBy(o => o.Stage).OrderBy(s => s.Key))
            {
                if (this.Logger.IsEnabled(LogLevel.Debug))
                {
                    this.Logger.LogDebug(
                        10,//(int)ErrorCode.LifecycleStagesReport,
                        "Stage {Stage}: {Observers}",
                        GetStageName(stage.Key),
                        string.Join(", ", stage.Select(o => o.Name)));
                }
            }

            return base.OnStart(cancellationToken);
        }

        /// <inheritdoc />
        protected override void OnStartStageCompleted(int stage)
        {
            Interlocked.Exchange(ref highestCompletedStage, stage);
            base.OnStartStageCompleted(stage);
        }

        /// <inheritdoc />
        protected override void OnStopStageCompleted(int stage)
        {
            Interlocked.Exchange(ref lowestStoppedStage, stage);
            base.OnStopStageCompleted(stage);
        }

        /// <inheritdoc />
        protected override string GetStageName(int stage)
        {
            if (StageNames.TryGetValue(stage, out var result)) return result;
            return base.GetStageName(stage);
        }

        /// <inheritdoc />
        protected override void PerfMeasureOnStop(int stage, TimeSpan elapsed)
        {
            if (this.Logger.IsEnabled(LogLevel.Debug))
            {
                this.Logger.LogDebug(
                    10,//(int)ErrorCode.SiloStartPerfMeasure,
                    "Stopping lifecycle stage '{Stage}' took '{Elapsed}'.",
                    GetStageName(stage),
                    elapsed);
            }
        }

        /// <inheritdoc />
        protected override void PerfMeasureOnStart(int stage, TimeSpan elapsed)
        {
            if (this.Logger.IsEnabled(LogLevel.Debug))
            {
                this.Logger.LogDebug(
                    10,//(int)ErrorCode.SiloStartPerfMeasure,
                    "Starting lifecycle stage '{Stage}' took '{Elapsed}'",
                    GetStageName(stage),
                    elapsed);
            }
        }

        /// <inheritdoc />
        public override IDisposable Subscribe(string observerName, int stage, ILifecycleObserver observer)
        {
            var monitoredObserver = new MonitoredObserver(observerName, stage, GetStageName(stage), observer, this.Logger);
            observers.Add(monitoredObserver);
            return base.Subscribe(observerName, stage, monitoredObserver);
        }

        private class MonitoredObserver : ILifecycleObserver
        {
            private readonly ILifecycleObserver observer;
            private readonly ILogger logger;

            public MonitoredObserver(string name, int stage, string stageName, ILifecycleObserver observer, ILogger logger)
            {
                Name = name;
                Stage = stage;
                StageName = stageName;
                this.observer = observer;
                this.logger = logger;
            }

            public string Name { get; }
            public int Stage { get; }
            public string StageName { get; }

            public async Task OnStart(CancellationToken ct)
            {
                try
                {
                    var stopwatch = ValueStopwatch.StartNew();
                    await observer.OnStart(ct);
                    stopwatch.Stop();
                    if (logger.IsEnabled(LogLevel.Debug))
                    {
                        logger.LogDebug(
                            10,//(int)ErrorCode.SiloStartPerfMeasure,
                            "'{Name}' started in stage '{Stage}' in '{Elapsed}'.",
                            Name,
                            StageName,
                            stopwatch.Elapsed);
                    }
                }
                catch (Exception exception)
                {
                    logger.LogError(
                        10,//(int)ErrorCode.LifecycleStartFailure,
                        exception,
                        "'{Name}' failed to start due to errors at stage '{Stage}'.",
                        Name,
                        StageName);
                    throw;
                }
            }

            public async Task OnStop(CancellationToken cancellationToken = default)
            {
                var stopwatch = ValueStopwatch.StartNew();
                try
                {
                    if (logger.IsEnabled(LogLevel.Debug))
                    {
                        logger.LogDebug(
                            10,//(int)ErrorCode.SiloStartPerfMeasure,
                            "'{Name}' stopping in stage '{Stage}'.",
                            Name,
                            StageName);
                    }

                    await observer.OnStop(cancellationToken);
                    stopwatch.Stop();
                    if (stopwatch.Elapsed > TimeSpan.FromSeconds(1))
                    {
                        logger.LogWarning(
                            10,//(int)ErrorCode.SiloStartPerfMeasure,
                            "'{Name}' stopped in stage '{Stage}' in '{Elapsed}'.",
                            Name,
                            StageName,
                            stopwatch.Elapsed);
                    }
                    else if (logger.IsEnabled(LogLevel.Debug))
                    {
                        logger.LogDebug(
                            10,//(int)ErrorCode.SiloStartPerfMeasure,
                            "'{Name}' stopped in stage '{Stage}' in '{Elapsed}'.",
                            Name,
                            StageName,
                            stopwatch.Elapsed);
                    }
                }
                catch (Exception exception)
                {
                    logger.LogError(
                        10,//(int)ErrorCode.LifecycleStartFailure,
                        exception,
                        "'{Name}' failed to stop due to errors at stage '{Stage}' after '{Elapsed}'.",
                        Name,
                        StageName,
                        stopwatch.Elapsed);
                    throw;
                }
            }
        }
    }
}
