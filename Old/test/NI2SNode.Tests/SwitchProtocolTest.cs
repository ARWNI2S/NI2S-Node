using NI2S.Node.Hosting;
using NI2S.Node.Protocol;
using System.Buffers;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace NI2S.Node.Tests
{
    [Trait("Category", "Protocol.SwitchProtocol")]
    public class SwitchProtocolTest : ProtocolTestBase
    {
        private static readonly Random _rd = new();

        private const char _beginFlagA = '$';
        private const char _beginFlagB = '*';

        class SwitchPipelineFilter : PipelineFilterBase<TextPackageInfo>
        {
            private readonly IPipelineFilter<TextPackageInfo> _filterA;

            private readonly IPipelineFilter<TextPackageInfo> _filterB;

            public SwitchPipelineFilter()
            {
                _filterA = new MyFixedSizePipelineFilter(this);
                _filterB = new MyFixedHeaderPipelineFilter(this);
            }

            public override TextPackageInfo Filter(ref SequenceReader<byte> reader)
            {
                if (!reader.TryRead(out byte flag))
                    throw new ProtocolException("Flag byte cannot be read.");

                if (flag == _beginFlagA)
                    NextFilter = _filterA;
                else if (flag == _beginFlagB)
                    NextFilter = _filterB;
                else
                    throw new ProtocolException($"Unknown flag at the first postion: {flag}.");

                return null;
            }
        }


        public SwitchProtocolTest(ITestOutputHelper outputHelper) : base(outputHelper)
        {

        }

        class MyFixedSizePipelineFilter : FixedSizePipelineFilter<TextPackageInfo>
        {
            public IPipelineFilter<TextPackageInfo> SwitchFilter { get; }

            public MyFixedSizePipelineFilter(IPipelineFilter<TextPackageInfo> switchFilter)
                : base(36)
            {
                SwitchFilter = switchFilter;
            }

            protected override TextPackageInfo DecodePackage(ref ReadOnlySequence<byte> buffer)
            {
                NextFilter = SwitchFilter;
                // skip the switch flag
                return new TextPackageInfo { Text = buffer.GetString(Encoding.UTF8) };
            }
        }

        class MyFixedHeaderPipelineFilter : FixedHeaderPipelineFilter<TextPackageInfo>
        {
            public IPipelineFilter<TextPackageInfo> SwitchFilter { get; }

            public MyFixedHeaderPipelineFilter(IPipelineFilter<TextPackageInfo> switchFilter)
                : base(4)
            {
                SwitchFilter = switchFilter;
            }

            protected override int GetBodyLengthFromHeader(ref ReadOnlySequence<byte> buffer)
            {
                // skip the switch flag
                var strLen = buffer.GetString(Utf8Encoding);
                return int.Parse(strLen.TrimStart('0'));
            }

            protected override TextPackageInfo DecodePackage(ref ReadOnlySequence<byte> buffer)
            {
                NextFilter = SwitchFilter;
                return new TextPackageInfo { Text = buffer.Slice(4).GetString(Utf8Encoding) };
            }
        }

        protected override string CreateRequest(string sourceLine)
        {
            if (_rd.Next() % 2 == 0)
            {
                return _beginFlagA + sourceLine;
            }
            else
            {
                return _beginFlagB + sourceLine.Length.ToString().PadLeft(4) + sourceLine;
            }
        }

        protected override INode CreateServer(IHostConfigurator hostConfigurator)
        {
            var server = CreateSocketServerBuilder<TextPackageInfo, SwitchPipelineFilter>(hostConfigurator)
                .UsePackageHandler(async (s, p) =>
                {
                    await s.SendAsync(Utf8Encoding.GetBytes(p.Text + "\r\n"));
                }).BuildAsServer() as INode;

            return server;
        }

        [Theory]
        [InlineData(typeof(RegularHostConfigurator))]
        public override async Task TestNormalRequest(Type hostConfiguratorType)
        {
            var hostConfigurator = CreateObject<IHostConfigurator>(hostConfiguratorType);

            using var server = CreateServer(hostConfigurator);
            await server.StartAsync();

            using (var socket = CreateClient(hostConfigurator))
            {
                using var socketStream = await hostConfigurator.GetClientStream(socket);
                using var reader = hostConfigurator.GetStreamReader(socketStream, Utf8Encoding);
                using var writer = new ConsoleWriter(socketStream, Utf8Encoding, 1024 * 8);
                var line = Guid.NewGuid().ToString();
                writer.Write(CreateRequest(line));
                writer.Flush();

                var receivedLine = await reader.ReadLineAsync();
                Assert.Equal(line, receivedLine);
            }

            await server.StopAsync();
        }
    }
}
