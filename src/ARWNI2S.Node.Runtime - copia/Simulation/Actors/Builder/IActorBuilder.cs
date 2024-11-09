using ARWNI2S.Runtime.Simulation.Objects.Builder;

namespace ARWNI2S.Runtime.Simulation.Actors.Builder
{
    public interface IActorBuilder : INI2SObjectBuilder
    {
    }

    public interface IActorBuilder<TActor> : INI2SObjectBuilder<TActor> where TActor : NI2SActor
    {
    }
}
