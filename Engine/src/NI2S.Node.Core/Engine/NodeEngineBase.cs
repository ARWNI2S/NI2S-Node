// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using System;
using System.Threading.Tasks;

namespace NI2S.Node.Engine
{
    public abstract class NodeEngineBase : INodeEngine
    {
        public IModuleCollection Modules { get; set; }

        public virtual void Run()
        {

        }

        public virtual Task RunAsync()
        {
            return Task.CompletedTask;
        }
    }
}
