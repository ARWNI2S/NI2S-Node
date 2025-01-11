using ARWNI2S.Engine.Core.Actor;
using ARWNI2S.Engine.Core.Object;

namespace ARWNI2S.Engine.Simulation
{
    internal class SimualtionRoot : NI2SObject, ISimulation
    {
        internal ActorId ActorId { get; }

        protected ISyncRootGrain Self { get; }
    }
}
