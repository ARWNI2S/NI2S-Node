// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using NI2S.Network.Protocol;
using System;
using System.Buffers;

namespace NI2S.Node.Network.Protocol
{
    internal class DataPackageDecoder : IPackageDecoder<DataPackage>
    {
        public DataPackage Decode(ref ReadOnlySequence<byte> buffer, object context)
        {
            throw new NotImplementedException();
        }
    }
}
