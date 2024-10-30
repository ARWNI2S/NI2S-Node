using ARWNI2S.Runtime.Services.Clustering;
using ARWNI2S.Runtime.Simulation.Actors.Grains;
using ARWNI2S.Runtime.Simulation.Objects;

namespace ARWNI2S.Runtime.Simulation.Actors
{
    internal class NI2SActorBuilder : NI2SObjectBuilder<NI2SActor>
    {
        private readonly IDistributedActorHelper _distributedActorHelper;

        private Guid _actorUUID;

        public NI2SActorBuilder(IDistributedActorHelper distributedActorHelper)
        {
            _distributedActorHelper = distributedActorHelper;
        }

        public void LoadArchetype(string archetype)
        {
        }


        public override Guid Build(NI2SActor gameObject)
        {
            _actorUUID = GenerateForArchetype();
            return _actorUUID;
        }

        internal INI2SActorGrain GetDistributedActor(Guid uUID)
        {
            return _distributedActorHelper.GetDistributedActor(uUID);
        }

        private Guid GenerateForArchetype()
        {
            return new Guid();
        }

    }
}
