// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using NI2S.Node.Network.Protocol.Package;
using System;

namespace NI2S.Node.Network.Protocol
{
    [Serializable]
    public class DataPackage
    {
        public Header Header { get; set; }

        public short Sequence { get; set; }

        public string Body { get; set; }
    }
}
