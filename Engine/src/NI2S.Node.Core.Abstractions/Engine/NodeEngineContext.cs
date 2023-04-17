// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using System;

namespace NI2S.Node.Engine
{
    public abstract class NodeEngineContext
    {
        public IServiceProvider NodeServices { get; set; }
    }
}