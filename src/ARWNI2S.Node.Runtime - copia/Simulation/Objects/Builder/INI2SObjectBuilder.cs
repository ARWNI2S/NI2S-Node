using ARWNI2S.Engine.Simulation.Simulable.Builder;
using ARWNI2S.Runtime.Simulation.Actors;

namespace ARWNI2S.Runtime.Simulation.Objects.Builder
{
    public interface INI2SObjectBuilder : ISimulableObjectBuilder
    {
    }

    public interface INI2SObjectBuilder<TActor> : ISimulableObjectBuilder<TActor> where TActor : NI2SActor
    {
    }
}
