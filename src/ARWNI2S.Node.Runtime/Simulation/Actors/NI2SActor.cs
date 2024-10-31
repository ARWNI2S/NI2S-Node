using ARWNI2S.Runtime.Simulation.Objects;

namespace ARWNI2S.Runtime.Simulation.Actors
{
    internal abstract class NI2SActor : NI2SObject
    {
        protected override object Id => UUID;

        public string ArchetypeName { get; internal set; }

        //private INI2SActorGrain actorGrain;

        public override void BeginPlay() { }

    }
}
