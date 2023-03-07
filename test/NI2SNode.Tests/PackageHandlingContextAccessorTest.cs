using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Command;
using NI2S.Node.Hosting;
using NI2S.Node.Protocol;
using NI2S.Node.Protocol.Session;
using NI2S.Node.Server;
using System.Net.Sockets;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace NI2S.Node.Tests
{
    [Trait("Category", "PackageHandlingContextAccessor")]
    public class PackageHandlingContextAccessorTest : TestClassBase
    {
        public PackageHandlingContextAccessorTest(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }

        [Fact]
        public async Task TestUsePackageHandlingContextAccessor()
        {

            INodeHostBuilder<StringPackageInfo> superSocketHostBuilder = CreateSocketServerBuilder<StringPackageInfo, CommandLinePipelineFilter>()
                            .UseCommand(commandOptions =>
                            {
                                commandOptions.AddCommand<TestCommand>();
                            })
                            .UsePackageHandlingContextAccessor();
            using var server = superSocketHostBuilder.BuildAsServer();
            var packageHandlingContextAccessor = server.ServiceProvider.GetService<IPackageHandlingContextAccessor<StringPackageInfo>>();
            Assert.NotNull(packageHandlingContextAccessor);
            Assert.Equal("TestServer", server.Name);

            Assert.True(await server.StartAsync());
            OutputHelper.WriteLine("Server started.");


            var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await client.ConnectAsync(GetDefaultServerEndPoint());
            OutputHelper.WriteLine("Connected.");

            await Task.Delay(1000);

            using (var stream = new NetworkStream(client))
            using (var streamReader = new StreamReader(stream, Utf8Encoding, true))
            using (var streamWriter = new StreamWriter(stream, Utf8Encoding, 1024 * 1024 * 4))
            {
                await streamWriter.WriteAsync("TestCommand Package 1\r\n");
                await streamWriter.FlushAsync();
                OutputHelper.WriteLine("send 'Package 1'.");
                var line = await streamReader.ReadLineAsync();
                Assert.Equal("Package 1", line);

                await streamWriter.WriteAsync("TestCommand Package 2\r\n");
                await streamWriter.FlushAsync();
                OutputHelper.WriteLine("send 'Package 2'.");
                line = await streamReader.ReadLineAsync();
                Assert.Equal("Package 2", line);

            }

            await server.StopAsync();
        }

        public class TestCommand : IAsyncCommand<StringPackageInfo>
        {
            private readonly IServiceProvider serviceProvider;

            public TestCommand(IServiceProvider serviceProvider)
            {
                this.serviceProvider = serviceProvider;
            }


            public async ValueTask ExecuteAsync(ISession session, StringPackageInfo package)
            {
                var packageHandlingContextAccessor = serviceProvider.GetService<IPackageHandlingContextAccessor<StringPackageInfo>>();
                if (packageHandlingContextAccessor != null)
                {
                    Assert.NotNull(packageHandlingContextAccessor.PackageHandlingContext.AppSession);
                    Assert.NotNull(packageHandlingContextAccessor.PackageHandlingContext.PackageInfo);

                    Assert.Equal(packageHandlingContextAccessor.PackageHandlingContext.AppSession, session);
                    Assert.Equal(packageHandlingContextAccessor.PackageHandlingContext.PackageInfo, package);
                    await session.SendAsync(Encoding.UTF8.GetBytes(package.Body + "\r\n"));
                }
            }
        }
    }


}
