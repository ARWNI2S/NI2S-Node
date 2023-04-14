// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using NI2S.Node.Engine;

namespace NI2S.Node.Engine
{
    internal class DefaultWorkContext : IWorkContext
    {
        public IModuleCollection Modules => throw new System.NotImplementedException();

        //public NodeMessage Message => throw new System.NotImplementedException();

        public string TraceIdentifier => throw new System.NotImplementedException();
    }
}