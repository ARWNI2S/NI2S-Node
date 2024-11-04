using ARWNI2S.Runtime.Simulation.Actors.Grains;

namespace ARWNI2S.Runtime.Services.Clustering
{
    public interface IDistributedActorHelper
    {
        INI2SActorGrain GetDistributedActor(Guid uUID);
    }
}
