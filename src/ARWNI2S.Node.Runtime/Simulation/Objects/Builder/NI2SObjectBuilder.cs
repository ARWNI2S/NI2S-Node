using ARWNI2S.Engine.Simulation.Simulable;
using ARWNI2S.Engine.Simulation.Simulable.Builder;
using ARWNI2S.Engine.Simulation.Simulable.Factory;

namespace ARWNI2S.Runtime.Simulation.Objects.Builder
{
    internal class NI2SObjectBuilder : INI2SObjectBuilder
    {
        public ISimulableObjectFactory Factory => throw new NotImplementedException();

        public Type SimulableType => throw new NotImplementedException();

        ISimulable ISimulableObjectBuilder.SimulableObject { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void BeginComposition()
        {
            throw new NotImplementedException();
        }

        public Guid Build()
        {
            throw new NotImplementedException();
        }

        public void FinalizeComposition()
        {
            throw new NotImplementedException();
        }
    }
}
