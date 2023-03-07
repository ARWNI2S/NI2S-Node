using NI2S.Node.Protocol;
using NI2S.Node.Protocol.Channel;

namespace NI2S.Node.Tests
{
    public class UdpTextReader : TextReader
    {
        public UdpPipeChannel<TextPackageInfo> Channel { get; }

        private readonly IAsyncEnumerator<TextPackageInfo> _packageEnumerator;

        public UdpTextReader(UdpPipeChannel<TextPackageInfo> channel)
        {
            Channel = channel;
            _packageEnumerator = channel.RunAsync().GetAsyncEnumerator();
        }

        public override string ReadLine()
        {
            return ReadLineAsync().GetAwaiter().GetResult();
        }

        public async override Task<string> ReadLineAsync()
        {
            var ret = await _packageEnumerator.MoveNextAsync().ConfigureAwait(false);

            if (!ret)
                return null;

            return _packageEnumerator.Current?.Text;
        }
    }
}