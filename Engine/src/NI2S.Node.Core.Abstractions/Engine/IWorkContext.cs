// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

namespace NI2S.Node.Engine
{
    public interface IWorkContext
    {
        IModuleCollection Modules { get; }

        string TraceIdentifier { get; }
    }
}
