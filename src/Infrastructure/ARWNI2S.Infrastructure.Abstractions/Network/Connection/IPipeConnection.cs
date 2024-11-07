﻿using ARWNI2S.Infrastructure.Network.Protocol;
using System.IO.Pipelines;

namespace ARWNI2S.Infrastructure.Network.Connection
{
    public interface IPipeConnection
    {
        IPipelineFilter PipelineFilter { get; }

        PipeReader InputReader { get; }

        PipeWriter OutputWriter { get; }
    }
}
