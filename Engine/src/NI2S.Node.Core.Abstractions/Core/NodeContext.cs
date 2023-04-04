// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using System;

namespace NI2S.Node.Core
{
    public abstract class NodeContext
    {
        public IServiceProvider NodeServices { get; set; }
    }
}