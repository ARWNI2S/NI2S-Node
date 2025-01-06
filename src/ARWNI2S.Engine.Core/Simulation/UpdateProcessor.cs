using ARWNI2S.Lifecycle;

namespace ARWNI2S.Engine.Simulation
{
    public abstract class UpdateProcessor : INiisProcessor
    {
        public abstract void Participate(IEngineLifecycle lifecycle);

        protected virtual Task OnStart(CancellationToken token)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnStop(CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}
