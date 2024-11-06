﻿using ARWNI2S.Node.Core.Network.Protocol;
using Microsoft.Extensions.Options;
using SuperSocket;
using SuperSocket.Connection;
using SuperSocket.ProtoBase;
using SuperSocket.Server;
using SuperSocket.Server.Abstractions;
using SuperSocket.Server.Abstractions.Session;

namespace ARWNI2S.Runtime.Network
{
    internal class NI2SFrontlineService : SuperSocketService<NI2SProtoPacket>
    {
        public NI2SFrontlineService(IServiceProvider serviceProvider, IOptions<ServerOptions> serverOptions)
            : base(serviceProvider, serverOptions)
        {
        }

        protected override object CreatePipelineContext(IAppSession session)
        {
            return base.CreatePipelineContext(session);
        }

        protected override ValueTask FireSessionClosedEvent(AppSession session, CloseReason reason)
        {
            return base.FireSessionClosedEvent(session, reason);
        }

        protected override ValueTask FireSessionConnectedEvent(AppSession session)
        {
            return base.FireSessionConnectedEvent(session);
        }

        protected override CancellationTokenSource GetPackageHandlingCancellationTokenSource(CancellationToken cancellationToken)
        {
            return base.GetPackageHandlingCancellationTokenSource(cancellationToken);
        }

        protected override IPipelineFilterFactory<NI2SProtoPacket> GetPipelineFilterFactory()
        {
            return base.GetPipelineFilterFactory();
        }

        protected override ValueTask OnNewConnectionAccept(ListenOptions listenOptions, IConnection connection)
        {
            return base.OnNewConnectionAccept(listenOptions, connection);
        }

        protected override ValueTask OnSessionClosedAsync(IAppSession session, CloseEventArgs e)
        {
            return base.OnSessionClosedAsync(session, e);
        }

        protected override ValueTask OnSessionConnectedAsync(IAppSession session)
        {
            return base.OnSessionConnectedAsync(session);
        }

        protected override ValueTask<bool> OnSessionErrorAsync(IAppSession session, PackageHandlingException<NI2SProtoPacket> exception)
        {
            return base.OnSessionErrorAsync(session, exception);
        }

        protected override ValueTask OnStartedAsync()
        {
            return base.OnStartedAsync();
        }

        protected override ValueTask OnStopAsync()
        {
            return base.OnStopAsync();
        }
    }
}
