// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using System;

namespace NI2S.Node.Engine
{
    public abstract class EngineContext : IWorkContext
    {
        public IServiceProvider NodeServices { get; set; }

        public IModuleCollection Modules { get; set; }

        public string TraceIdentifier { get; set; }
    }
}