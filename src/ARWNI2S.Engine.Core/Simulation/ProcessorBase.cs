using ARWNI2S.Engine.Environment;
using ARWNI2S.Engine.Threading;
using ARWNI2S.Lifecycle;
using Microsoft.Extensions.Logging;
using Orleans;

namespace ARWNI2S.Engine.Simulation
{
    public abstract class ProcessorBase : INiisProcessor
    {
        private IThreadManager _threadManager;
        private readonly ILogger _Logger;

        public bool IsInitialized => _threadManager.DedicatedThread != null;
        public bool IsRunning => _threadManager.DedicatedThread != null && _threadManager.DedicatedThread.IsAlive;

        public ProcessorBase(ILogger logger)
        {
            _Logger = logger;
        }

        public virtual void Participate(IEngineLifecycle lifecycle)
        {
            lifecycle.Subscribe(GetType().Name, NI2SLifecycleStage.RuntimeInitialize, OnInitialize, OnDeinitialize);
            lifecycle.Subscribe(GetType().Name, NI2SLifecycleStage.RuntimeServices, OnStart, OnStop);
        }

        private Task OnInitialize(CancellationToken token)
        {
            _Logger.LogTrace("Initializing {ProcessorType}", GetType().Name);

            _threadManager = ThreadManager.RegisterProcessor(this);

            return Task.Run(Initialize, token);
        }

        protected virtual void Initialize() { /* Do nothing */ }

        private Task OnStart(CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return Task.CompletedTask;

            _Logger.LogTrace("Starting {ProcessorType}", GetType().Name);

            return Task.Run(_threadManager.Run, token);
        }

#pragma warning disable IDE0051 // Quitar miembros privados no utilizados
        private void ThreadStartImpl(CancellationToken cancelToken)
        {
            Starting();
            while (!cancelToken.IsCancellationRequested)
            {
                // Do work
            }
        }
#pragma warning restore IDE0051 // Quitar miembros privados no utilizados

        protected virtual void Starting() { /* Do nothing */ }

        private Task OnStop(CancellationToken token)
        {
            _Logger.LogTrace("Stopping {ProcessorType}", GetType().Name);

            Stopping();
            return Task.Run(_threadManager.End, token);
        }

        protected virtual void Stopping() { /* Do nothing */ }

        private Task OnDeinitialize(CancellationToken token)
        {
            _Logger.LogTrace("Deinitializing {ProcessorType}", GetType().Name);

            return Task.Run(Deinitialize, token);
        }

        protected virtual void Deinitialize() { /* Do nothing */ }

    }
}
