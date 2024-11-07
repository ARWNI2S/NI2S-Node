using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ARWNI2S.Infrastructure.Network.Protocol.ProxyProtocol
{
    class ProxyProtocolV1PartReader<TPackageInfo> : ProxyProtocolPackagePartReader<TPackageInfo>
    {
        private static readonly byte[] PROXY_DELIMITER = Encoding.ASCII.GetBytes("\r\n");

        private static readonly IProxySgementProcessor[] PROXY_SEGMENT_PARSERS = new IProxySgementProcessor[]
            {
                new SourceIPAddressProcessor(),
                new DestinationIPAddressProcessor(),
                new SourcePortProcessor(),
                new DestinationPortProcessor()
            };

        public override bool Process(TPackageInfo package, object filterContext, ref SequenceReader<byte> reader, out IPackagePartReader<TPackageInfo> nextPartReader, out bool needMoreData)
        {
            if (!reader.TryReadTo(out ReadOnlySequence<byte> proxyLineSequence, PROXY_DELIMITER, true))
            {
                needMoreData = true;
                nextPartReader = null;
                return false;
            }

            needMoreData = false;
            nextPartReader = null;

            var proxyLineReader = new SequenceReader<byte>(proxyLineSequence);

            var proxyLine = proxyLineReader.ReadString();

            var proxyInfo = filterContext as ProxyInfo;

            // "PROXY TCP4 X", start look for next segment from X(@11)
            LoadProxyInfo(proxyInfo, proxyLine, 11, 12);

            proxyInfo.Version = 1;
            proxyInfo.Command = ProxyCommand.PROXY;
            proxyInfo.ProtocolType = ProtocolType.Tcp;

            return true;
        }

        private void LoadProxyInfo(ProxyInfo proxyInfo, string line, int startPos, int lookForOffet)
        {
            var span = line.AsSpan();
            var segmentIndex = 0;

            while (lookForOffet < line.Length)
            {
                var spacePos = line.IndexOf(' ', lookForOffet);

                ReadOnlySpan<char> segment;

                if (spacePos >= 0)
                {
                    segment = span.Slice(startPos, spacePos - startPos);
                    startPos = spacePos + 1;
                    lookForOffet = startPos + 1;
                }
                else
                {
                    segment = span.Slice(startPos);
                    lookForOffet = line.Length;
                }

                PROXY_SEGMENT_PARSERS[segmentIndex++].Process(segment, proxyInfo);
            }
        }

        interface IProxySgementProcessor
        {
            void Process(ReadOnlySpan<char> segment, ProxyInfo proxyInfo);
        }

        class SourceIPAddressProcessor : IProxySgementProcessor
        {
            public void Process(ReadOnlySpan<char> segment, ProxyInfo proxyInfo)
            {
                proxyInfo.SourceIPAddress = IPAddress.Parse(segment);
            }
        }

        class DestinationIPAddressProcessor : IProxySgementProcessor
        {
            public void Process(ReadOnlySpan<char> segment, ProxyInfo proxyInfo)
            {
                proxyInfo.DestinationIPAddress = IPAddress.Parse(segment);
            }
        }

        class SourcePortProcessor : IProxySgementProcessor
        {
            public void Process(ReadOnlySpan<char> segment, ProxyInfo proxyInfo)
            {
                proxyInfo.SourcePort = int.Parse(segment);
            }
        }

        class DestinationPortProcessor : IProxySgementProcessor
        {
            public void Process(ReadOnlySpan<char> segment, ProxyInfo proxyInfo)
            {
                proxyInfo.DestinationPort = int.Parse(segment);
            }
        }
    }
}