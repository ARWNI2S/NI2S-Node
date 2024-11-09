using ARWNI2S.Infrastructure;
using ARWNI2S.Runtime.Simulation.Actors.Grains;
using ARWNI2S.Runtime.Simulation.World.Grains;

namespace ARWNI2S.Runtime.Services.Clustering
{
    public class DistributedActorHelper : IDistributedActorHelper
    {
        private readonly IGrainFactory _grainFactory;

        public DistributedActorHelper(IGrainFactory grainFactory)
        {
            _grainFactory = grainFactory;
        }

        public INI2SActorGrain GetDistributedActor(Guid uUID) => _grainFactory.GetGrain<INI2SActorGrain>(uUID);

        public INI2SGameWorldGrain GetDistributedGameWorld() => _grainFactory.GetGrain<INI2SGameWorldGrain>(Constants._UUID_WORLD_ACTOR);
    }
}
