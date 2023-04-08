// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using NI2S.Network.Protocol;
using System;
using System.Buffers;

namespace NI2S.Node.Network.Protocol
{
    internal class DataPackageEncoder : IPackageEncoder<DataPackage>
    {
        public int Encode(IBufferWriter<byte> writer, DataPackage pack)
        {
            throw new NotImplementedException();
        }
    }
}
