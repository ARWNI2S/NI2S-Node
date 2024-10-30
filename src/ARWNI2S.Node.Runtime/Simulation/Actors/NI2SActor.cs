using ARWNI2S.Engine.Simulation.Entities;
using ARWNI2S.Runtime.Simulation.Actors.Grains;
using ARWNI2S.Runtime.Simulation.Objects;

namespace ARWNI2S.Runtime.Simulation.Actors
{
    internal abstract class NI2SActor : NI2SObject
    {
        protected override object Id => UUID;

        public string ArchetypeName { get; internal set; }

        private INI2SActorGrain actorGrain;

        public override void BeginPlay() { }

        public virtual void InitializeActor(NI2SActorBuilder actorBuilder)
        {
            base.InitializeGameObject(actorBuilder);

            // Inicialización de actorGrain usando el builder o un factory
            actorGrain = actorBuilder.GetDistributedActor(UUID) as INI2SActorGrain
                         ?? throw new InvalidOperationException("ActorGrain could not be initialized.");


        }

    }
}
