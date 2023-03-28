﻿using NI2S.Node.Configuration;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace NI2S.Node.Networking.Channel
{
    public class SslStreamPipeChannel<TPackageInfo> : StreamPipeChannel<TPackageInfo>, IChannelWithRemoteCertificate
    {
        public SslStreamPipeChannel(SslStream stream, EndPoint remoteEndPoint, IPipelineFilter<TPackageInfo> pipelineFilter, ChannelOptions options)
            : this(stream, remoteEndPoint, null, pipelineFilter, options)
        {
        }

        public SslStreamPipeChannel(SslStream stream, EndPoint remoteEndPoint, EndPoint localEndPoint, IPipelineFilter<TPackageInfo> pipelineFilter, ChannelOptions options)
            : base(stream, remoteEndPoint, localEndPoint, pipelineFilter, options)
        {
            if (stream.IsAuthenticated || stream.IsMutuallyAuthenticated)
            {
                RemoteCertificate = stream.RemoteCertificate!;
            }
        }

        public X509Certificate RemoteCertificate { get; }
    }
}
