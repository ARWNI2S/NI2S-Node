using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NI2S.Node.Networking;
using NI2S.Node.Networking.Channel;
using NI2S.Node.Configuration.Options;
using NI2S.Node.Resources;
using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Security.Authentication;
using NI2S.Node.Client.Options;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NI2S.Node.Client
{
    public class NodeClient<TPackage, TSendPackage> : NodeClient<TPackage>, INodeClient<TPackage, TSendPackage>
        where TPackage : class
    {
        private readonly IPackageEncoder<TSendPackage> _packageEncoder;

        protected NodeClient(IPackageEncoder<TSendPackage> packageEncoder)
            : base()
        {
            _packageEncoder = packageEncoder;
        }

        public NodeClient(IPipelineFilter<TPackage> pipelineFilter, IPackageEncoder<TSendPackage> packageEncoder, ILogger logger = default!)
            : this(pipelineFilter, packageEncoder, new ChannelOptions { Logger = logger })
        {

        }

        public NodeClient(IPipelineFilter<TPackage> pipelineFilter, IPackageEncoder<TSendPackage> packageEncoder, ChannelOptions options)
            : base(pipelineFilter, options)
        {
            _packageEncoder = packageEncoder;
        }

        public virtual async ValueTask SendAsync(TSendPackage package)
        {
            await SendAsync(_packageEncoder, package);
        }

        public new INodeClient<TPackage, TSendPackage> AsClient()
        {
            return this;
        }
    }

    public class NodeClient<TReceivePackage> : INodeClient<TReceivePackage>
        where TReceivePackage : class
    {
        private readonly IPipelineFilter<TReceivePackage> _pipelineFilter;

        protected IChannel<TReceivePackage> Channel { get; private set; } = default!;

        protected ILogger Logger { get; set; }

        protected ChannelOptions Options { get; private set; }

        IAsyncEnumerator<TReceivePackage> _packageStream = default!;

        public event PackageHandler<TReceivePackage>? PackageHandler;

        public IPEndPoint LocalEndPoint { get; set; } = default!;

        public SecurityOptions Security { get; set; } = default!;

        protected NodeClient()
        {
            _pipelineFilter = default!;
            Options = default!;
            Logger = default!;
        }

        public NodeClient(IPipelineFilter<TReceivePackage> pipelineFilter)
            : this(pipelineFilter, NullLogger.Instance)
        {

        }

        public NodeClient(IPipelineFilter<TReceivePackage> pipelineFilter, ILogger logger)
            : this(pipelineFilter, new ChannelOptions { Logger = logger })
        {

        }

        public NodeClient(IPipelineFilter<TReceivePackage> pipelineFilter, ChannelOptions options)
        {
            ArgumentNullException.ThrowIfNull(pipelineFilter, nameof(pipelineFilter));
            ArgumentNullException.ThrowIfNull(options, nameof(options));

            _pipelineFilter = pipelineFilter;
            Options = options;
            Logger = options.Logger;
        }

        public virtual INodeClient<TReceivePackage> AsClient()
        {
            return this;
        }

        protected virtual IConnector GetConnector()
        {
            var security = Security;

            if (security != null)
            {
                if (security.EnabledSslProtocols != SslProtocols.None)
                    return new SocketConnector(LocalEndPoint, new SslStreamConnector(security));
            }

            return new SocketConnector(LocalEndPoint);
        }

        ValueTask<bool> INodeClient<TReceivePackage>.ConnectAsync(EndPoint remoteEndPoint, CancellationToken cancellationToken)
        {
            return ConnectAsync(remoteEndPoint, cancellationToken);
        }

        protected virtual async ValueTask<bool> ConnectAsync(EndPoint remoteEndPoint, CancellationToken cancellationToken)
        {
            var connector = GetConnector();
            var state = await connector.ConnectAsync(remoteEndPoint, null, cancellationToken);


            if (state == null || state.Cancelled || cancellationToken.IsCancellationRequested)
            {
                //TODO: CHeck Log
                OnError(state?.Exception, LocalizedStrings.Error_ConnectionCancelled_Format, remoteEndPoint);
                return false;
            }

            if (!state.Result)
            {
                //TODO: CHeck Log
                OnError(state.Exception, LocalizedStrings.Error_FailedToConnect_Format, remoteEndPoint);
                return false;
            }

            _ = state.Socket ?? throw new Exception("Socket is null.");
            var channelOptions = Options;
            SetupChannel(state.CreateChannel<TReceivePackage>(_pipelineFilter, channelOptions));
            return true;
        }

        public void AsUdp(IPEndPoint remoteEndPoint, ArrayPool<byte>? bufferPool = null, int bufferSize = 4096)
        {
            var localEndPoint = LocalEndPoint;

            localEndPoint ??= new IPEndPoint(remoteEndPoint.AddressFamily == AddressFamily.InterNetworkV6 ? IPAddress.IPv6Any : IPAddress.Any, 0);

            var socket = new Socket(remoteEndPoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

            // bind the local endpoint
            socket.Bind(localEndPoint);

            var channel = new UdpPipeChannel<TReceivePackage>(socket, _pipelineFilter, this.Options, remoteEndPoint);

            SetupChannel(channel);

            UdpReceive(socket, channel, bufferPool, bufferSize);
        }

        private async void UdpReceive(Socket socket, UdpPipeChannel<TReceivePackage> channel, ArrayPool<byte>? bufferPool, int bufferSize)
        {
            bufferPool ??= ArrayPool<byte>.Shared;

            while (true)
            {
                var buffer = bufferPool.Rent(bufferSize);

                try
                {
                    var result = await socket
                        .ReceiveFromAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), SocketFlags.None, channel.RemoteEndPoint)
                        .ConfigureAwait(false);

                    await channel.WritePipeDataAsync((new ArraySegment<byte>(buffer, 0, result.ReceivedBytes)).AsMemory(), CancellationToken.None);
                }
                catch (NullReferenceException)
                {
                    break;
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (Exception e)
                {
                    OnError($"Failed to receive UDP data.", e);
                }
                finally
                {
                    bufferPool.Return(buffer);
                }
            }
        }

        protected virtual void SetupChannel(IChannel<TReceivePackage> channel)
        {
            channel.Closed += OnChannelClosed;
            channel.Start();
            _packageStream = channel.GetPackageStream();
            Channel = channel;
        }

        ValueTask<TReceivePackage> INodeClient<TReceivePackage>.ReceiveAsync()
        {
            return ReceiveAsync();
        }

        /// <summary>
        /// Try to receive one package
        /// </summary>
        /// <returns></returns>
        protected virtual async ValueTask<TReceivePackage> ReceiveAsync()
        {
            var p = await _packageStream.ReceiveAsync();

            if (p != null)
                return p;

            OnClosed(Channel, EventArgs.Empty);
            return null!;
        }

        void INodeClient<TReceivePackage>.StartReceive()
        {
            StartReceive();
        }

        /// <summary>
        /// Start receive packages and handle the packages by event handler
        /// </summary>
        protected virtual void StartReceive()
        {
            StartReceiveAsync();
        }

        private async void StartReceiveAsync()
        {
            var enumerator = _packageStream;

            while (await enumerator.MoveNextAsync())
            {
                await OnPackageReceived(enumerator.Current);
            }
        }

        protected virtual async ValueTask OnPackageReceived(TReceivePackage package)
        {
            var handler = PackageHandler ?? throw new InvalidOperationException(string.Format(LocalizedStrings.Error_ObjectIsNull_Format, nameof(PackageHandler)));
            try
            {
                await handler.Invoke(this, package);
            }
            catch (Exception e)
            {
                OnError("Unhandled exception happened in PackageHandler.", e);
            }
        }

        private void OnChannelClosed(object? sender, EventArgs e)
        {
            Channel.Closed -= OnChannelClosed;
            OnClosed(this, e);
        }

        private void OnClosed(object sender, EventArgs e)
        {
            var handler = Closed;

            if (handler != null)
            {
                if (Interlocked.CompareExchange(ref Closed, null, handler) == handler)
                {
                    handler.Invoke(sender, e);
                }
            }
        }

        protected virtual void OnError(Exception? exception, string message, params object[] args)
        {
            Logger?.LogError(exception, message, args);
        }

        protected virtual void OnError(string message, params object[] args)
        {
            Logger?.LogError(message, args);
        }

        ValueTask INodeClient<TReceivePackage>.SendAsync(ReadOnlyMemory<byte> data)
        {
            return SendAsync(data);
        }

        protected virtual async ValueTask SendAsync(ReadOnlyMemory<byte> data)
        {
            await Channel.SendAsync(data);
        }

        ValueTask INodeClient<TReceivePackage>.SendAsync<TSendPackage>(IPackageEncoder<TSendPackage> packageEncoder, TSendPackage package)
        {
            return SendAsync<TSendPackage>(packageEncoder, package);
        }

        protected virtual async ValueTask SendAsync<TSendPackage>(IPackageEncoder<TSendPackage> packageEncoder, TSendPackage package)
        {
            await Channel.SendAsync(packageEncoder, package);
        }

        public event EventHandler? Closed;

        public virtual async ValueTask CloseAsync()
        {
            await Channel.CloseAsync(CloseReason.LocalClosing);
            OnClosed(this, EventArgs.Empty);
        }
    }
}