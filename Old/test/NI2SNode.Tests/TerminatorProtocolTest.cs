using NI2S.Node.Hosting;
using NI2S.Node.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace NI2S.Node.Tests
{
    [Trait("Category", "Protocol.Terminator")]
    public class TerminatorProtocolTest : ProtocolTestBase
    {
        public TerminatorProtocolTest(ITestOutputHelper outputHelper) : base(outputHelper)
        {

        }

        protected override string CreateRequest(string sourceLine)
        {
            return $"{sourceLine}##";
        }

        protected override INode CreateServer(IHostConfigurator hostConfigurator)
        {
            var server = CreateSocketServerBuilder((x) => new TerminatorTextPipelineFilter("##"u8.ToArray()), hostConfigurator)
                .UsePackageHandler(async (s, p) =>
                {
                    await s.SendAsync(Utf8Encoding.GetBytes(p.Text + "\r\n"));
                }).BuildAsServer() as INode;

            return server;
        }
    }
}
