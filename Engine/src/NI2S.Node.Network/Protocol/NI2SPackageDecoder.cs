// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using NI2S.Network.Protocol;
using System;
using System.Buffers;

namespace NI2S.Node.Network.Protocol
{
    internal class NI2SPackageDecoder : IPackageDecoder<NI2SPackage>
    {
        public NI2SPackage Decode(ref ReadOnlySequence<byte> buffer, object context)
        {
            throw new NotImplementedException();
        }
    }
}
