﻿using ARWNI2S.Node.Core.Network.Protocol;
using System.Buffers;

namespace ARWNI2S.Node.Core.Network.Pipeline
{
    public sealed class PipelineFilterRoot : NI2SPacketFilter
    {
        public override Protocol.NI2SProtoPacket Filter(ref SequenceReader<byte> reader)
        {
            throw new NotImplementedException();
        }
    }
}