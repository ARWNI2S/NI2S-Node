// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.


using System;

namespace NI2S.Node.Engine
{
    public class ModuleOptions
    {
        public IServiceProvider EngineServices { get; internal set; }

        public long DefaultStreamErrorCode { get; internal set; }

        public long DefaultCloseErrorCode { get; internal set; }
    }
}
