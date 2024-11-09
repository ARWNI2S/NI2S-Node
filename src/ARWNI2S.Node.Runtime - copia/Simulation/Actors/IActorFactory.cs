using ARWNI2S.Engine.Simulation.Simulable.Factory;

namespace ARWNI2S.Runtime.Simulation.Actors
{
    public interface IActorFactory : ISimulableObjectFactory
    {
        //internal void InitializeInternal(SimulableBase simulable);
    }

    public interface IActorFactory<TActor> : ISimulableObjectFactory<TActor> where TActor : NI2SActor
    {
        //internal void InitializeInternal(SimulableBase simulable);
    }
}