using ARWNI2S.Runtime.Simulation.Actors.Grains;
using ARWNI2S.Runtime.Simulation.Objects;

namespace ARWNI2S.Runtime.Simulation.Actors
{
    public abstract class NI2SActor : NI2SObject
    {
        protected override object Id => UUID;

        public string ArchetypeName { get; internal set; }

        protected internal INI2SActorGrain actorGrain;

        public override void BeginPlay() { }

    }
}
