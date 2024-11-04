using ARWNI2S.Engine.Simulation.World;
using ARWNI2S.Runtime.Simulation.World.Grains;

namespace ARWNI2S.Runtime.Simulation.World
{
    internal sealed class NI2SGameWorld : SimulationWorld, IWorld
    {
        protected internal INI2SGameWorldGrain worldGrain;
    }
}
