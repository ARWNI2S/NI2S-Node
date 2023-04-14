// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using System.Threading.Tasks;

namespace NI2S.Node.Engine
{
    public interface INodeEngine
    {
        IModuleCollection Modules { get; set; }

        void Run();

        Task RunAsync();
    }
}
