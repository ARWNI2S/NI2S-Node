using ARWNI2S.Runtime.Simulation.Actors.Grains;

namespace ARWNI2S.Runtime.Services.Clustering
{
    public class DefaultDistributedActorHelper : IDistributedActorHelper
    {
        private readonly IGrainFactory _grainFactory;

        public DefaultDistributedActorHelper(IGrainFactory grainFactory)
        {
            _grainFactory = grainFactory;
        }

        public INI2SActorGrain GetDistributedActor(Guid uUID)
        {
            return _grainFactory.GetGrain<INI2SActorGrain>(uUID);
        }
    }
}
