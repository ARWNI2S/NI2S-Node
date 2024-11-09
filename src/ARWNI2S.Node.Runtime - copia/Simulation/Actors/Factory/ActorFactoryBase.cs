using ARWNI2S.Runtime.Services.Clustering;

namespace ARWNI2S.Runtime.Simulation.Actors.Factory
{
    public abstract class ActorFactoryBase : IActorFactory
    {
        private readonly IDistributedActorHelper _distributedActorHelper;

        ActorFactoryBase(IDistributedActorHelper distributedActorHelper)
        {
            _distributedActorHelper = distributedActorHelper;
        }

        public abstract Type SimulableType { get; }

        //void IActorFactory.InitializeInternal(SimulableBase simulable)
        //{
        //    if(simulable is NI2SActor actor)
        //    {
        //        actor.InitializeInternal(_distributedActorHelper);
        //    }
        //}
    }
}
