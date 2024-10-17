using ARWNI2S.Infrastructure.Memory;

namespace ARWNI2S.Node.Runtime.Kernel.Events
{
    internal class EventPool : ObjectPool<SimulationEvent>, IEventPool
    {
        protected override SimulationEvent CreateNewPoolObject()
        {
            return new SimulationEvent();
        }

        protected override void ReleaseObject(SimulationEvent obj)
        {
            base.ReleaseObject(obj);
        }
    }
}
