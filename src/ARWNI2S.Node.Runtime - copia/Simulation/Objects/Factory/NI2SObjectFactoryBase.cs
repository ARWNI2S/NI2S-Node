using ARWNI2S.Engine.Simulation.Simulable.Factory;

namespace ARWNI2S.Runtime.Simulation.Objects.Factory
{
    public abstract class NI2SObjectFactoryBase : ISimulableObjectFactory
    {
        public Type SimulableType => throw new NotImplementedException();
    }
}
