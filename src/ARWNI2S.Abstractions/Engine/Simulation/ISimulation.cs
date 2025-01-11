using ARWNI2S.Engine.Core;

namespace ARWNI2S.Engine.Simulation
{
    public interface ISimulation : INiisEntity
    {
        virtual new Guid Id => NI2SConstants.SimulationRootId;
    }
}
