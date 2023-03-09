using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Client;
using NI2S.Node.Configuration.Options;
using NI2S.Node.Hosting;
using NI2S.Node.Protocol;
using NI2S.Node.Protocol.Channel;
using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NI2S.Node.Tests
{
    public class UdpHostConfigurator : IHostConfigurator
    {
        private static readonly ArrayPool<byte> _bufferPool = ArrayPool<byte>.Shared;

        public string WebSocketSchema => "ws";

        public bool IsSecure => false;

        public ListenOptions Listener { get; private set; }

        private static readonly Random _rd = new Random();

        public void Configure(INodeHostBuilder hostBuilder)
        {
            hostBuilder
                .UseUdp()
                .ConfigureServices((ctx, services) =>
                    {
                        services.Configure<ServerOptions>((options) =>
                        {
                            var listener = options.Listeners[0];
                            Listener = listener;
                        });
                    }
                );
        }

        public INodeClient<TPackageInfo> ConfigureNodeClient<TPackageInfo>(IPipelineFilter<TPackageInfo> pipelineFilter, ChannelOptions options) where TPackageInfo : class
        {
            return new NodeClient<TPackageInfo>(pipelineFilter, options);
        }

        public Socket CreateClient()
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            var retry = 0;

            while (retry < 10)
            {
                try
                {
                    var localPort = _rd.Next(40000, 50000);
                    var localEndPoint = new IPEndPoint(IPAddress.Loopback, localPort);
                    socket.Bind(localEndPoint);
                    return socket;
                }
                catch
                {
                    retry++;
                }
            }

            throw new Exception("Cannot find an available port for udp client binding.");
        }

        private async Task UdpReceive(Socket socket, IVirtualChannel channel)
        {
            var remoteEndPoint = new IPEndPoint(IPAddress.Any, 4040);

            while (true)
            {
                var buffer = _bufferPool.Rent(1024 * 5);

                try
                {
                    var result = await socket
                        .ReceiveFromAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), SocketFlags.None, remoteEndPoint)
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
                finally
                {
                    _bufferPool.Return(buffer);
                }
            }
        }

        public ValueTask<Stream> GetClientStream(Socket socket)
        {
            var channel = new UdpPipeChannel<TextPackageInfo>(socket, new TerminatorPipelineFilter<TextPackageInfo>(new[] { (byte)'\r', (byte)'\n' })
            {
                Decoder = new UdpPackageDecoder()
            },
                new ChannelOptions(), new IPEndPoint(IPAddress.Loopback, Listener.GetListenEndPoint().Port));

            channel.Start();

            UdpReceive(socket, channel).DoNotAwait();

            return new ValueTask<Stream>(new UdpChannelStream(channel, socket));
        }

        public TextReader GetStreamReader(Stream stream, Encoding encoding)
        {
            var channel = (stream as UdpChannelStream).Channel;
            return new UdpTextReader(channel);
        }

        class UdpPackageDecoder : IPackageDecoder<TextPackageInfo>
        {
            public TextPackageInfo Decode(ref ReadOnlySequence<byte> buffer, object context)
            {
                return new TextPackageInfo { Text = buffer.GetString(Encoding.UTF8) };
            }
        }

        public async ValueTask KeepSequence()
        {
            await Task.Delay(200);
        }

    }
}