using System;
using System.Text;
using Microsoft.Extensions.Hosting;
using NI2S.Node;
using NI2S.Node.Protocol;
using NI2S.Node.Server;
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
            var server = CreateSocketServerBuilder<TextPackageInfo>((x) => new TerminatorTextPipelineFilter(new[] { (byte)'#', (byte)'#' }), hostConfigurator)
                .UsePackageHandler(async (s, p) =>
                {
                    await s.SendAsync(Utf8Encoding.GetBytes(p.Text + "\r\n"));
                }).BuildAsServer() as INode;

            return server;
        }
    }
}
