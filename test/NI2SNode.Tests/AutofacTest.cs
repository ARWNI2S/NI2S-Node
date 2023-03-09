using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NI2S.Node.Command;
using NI2S.Node.Hosting;
using NI2S.Node.Protocol;
using NI2S.Node.Protocol.Session;
using System.Net;
using System.Net.Sockets;
using Xunit;
using Xunit.Abstractions;

namespace NI2S.Node.Tests
{
    [Trait("Category", "Autofac")]
    public class AutofacTest : TestClassBase
    {
        public AutofacTest(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {

        }

        [Theory]
        [InlineData(typeof(RegularHostConfigurator))]
        [InlineData(typeof(SecureHostConfigurator))]
        [Trait("Category", "Autofac.Commands")]
        public async Task TestCommands(Type hostConfiguratorType)
        {
            var hostConfigurator = CreateObject<IHostConfigurator>(hostConfiguratorType);
            using var server = CreateSocketServerBuilder<StringPackageInfo, CommandLinePipelineFilter>(hostConfigurator)
                .UseCommand()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterType<ADD>().As<IAsyncCommand<ISession, StringPackageInfo>>();
                    builder.RegisterType<MULT>().As<IAsyncCommand<ISession, StringPackageInfo>>();
                    builder.RegisterType<SUB>().As<IAsyncCommand<ISession, StringPackageInfo>>();
                })
                .BuildAsServer();

            Assert.Equal("TestServer", server.Name);

            Assert.True(await server.StartAsync());
            OutputHelper.WriteLine("Server started.");


            var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await client.ConnectAsync(new IPEndPoint(IPAddress.Loopback, hostConfigurator.Listener.Port));
            OutputHelper.WriteLine("Connected.");

            using (var stream = await hostConfigurator.GetClientStream(client))
            using (var streamReader = new StreamReader(stream, Utf8Encoding, true))
            using (var streamWriter = new StreamWriter(stream, Utf8Encoding, 1024 * 1024 * 4))
            {
                await streamWriter.WriteAsync("ADD 1 2 3\r\n");
                await streamWriter.FlushAsync();
                var line = await streamReader.ReadLineAsync();
                Assert.Equal("6", line);

                await streamWriter.WriteAsync("MULT 2 5\r\n");
                await streamWriter.FlushAsync();
                line = await streamReader.ReadLineAsync();
                Assert.Equal("10", line);

                await streamWriter.WriteAsync("SUB 8 2\r\n");
                await streamWriter.FlushAsync();
                line = await streamReader.ReadLineAsync();
                Assert.Equal("6", line);
            }

            await server.StopAsync();
        }

        [Theory]
        [InlineData(typeof(RegularHostConfigurator))]
        [InlineData(typeof(SecureHostConfigurator))]
        [Trait("Category", "Autofac.CommandsWithCustomSession")]
        public async Task TestCommandsWithCustomSession(Type hostConfiguratorType)
        {
            var hostConfigurator = CreateObject<IHostConfigurator>(hostConfiguratorType);
            using var server = CreateSocketServerBuilder<StringPackageInfo, CommandLinePipelineFilter>(hostConfigurator)
                .UseCommand()
                .UseSession<MySession>()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterType<ADD>().As<IAsyncCommand<ISession, StringPackageInfo>>();
                    builder.RegisterType<MULT>().As<IAsyncCommand<ISession, StringPackageInfo>>();
                    builder.RegisterType<SUB>().As<IAsyncCommand<ISession, StringPackageInfo>>();
                    builder.RegisterType<DIV>().As<IAsyncCommand<MySession, StringPackageInfo>>();
                })
                .BuildAsServer();

            Assert.Equal("TestServer", server.Name);

            Assert.True(await server.StartAsync());
            OutputHelper.WriteLine("Server started.");


            var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await client.ConnectAsync(new IPEndPoint(IPAddress.Loopback, hostConfigurator.Listener.Port));
            OutputHelper.WriteLine("Connected.");

            using (var stream = await hostConfigurator.GetClientStream(client))
            using (var streamReader = new StreamReader(stream, Utf8Encoding, true))
            using (var streamWriter = new StreamWriter(stream, Utf8Encoding, 1024 * 1024 * 4))
            {
                await streamWriter.WriteAsync("ADD 1 2 3\r\n");
                await streamWriter.FlushAsync();
                var line = await streamReader.ReadLineAsync();
                Assert.Equal("6", line);

                await streamWriter.WriteAsync("MULT 2 5\r\n");
                await streamWriter.FlushAsync();
                line = await streamReader.ReadLineAsync();
                Assert.Equal("10", line);

                await streamWriter.WriteAsync("SUB 8 2\r\n");
                await streamWriter.FlushAsync();
                line = await streamReader.ReadLineAsync();
                Assert.Equal("6", line);

                await streamWriter.WriteAsync("DIV 8 2\r\n");
                await streamWriter.FlushAsync();
                line = await streamReader.ReadLineAsync();
                Assert.Equal("4", line);
            }

            await server.StopAsync();
        }

        [Fact]
        [Trait("Category", "Autofac.MultipleServerHost")]
        public async Task TestCommandsWithCustomSessionMultipleServerHost()
        {
            using var server = MultipleServerHostBuilder.Create()
                .ConfigureAppConfiguration((hostCtx, configApp) =>
                {
                    configApp.Sources.Clear();
                    configApp.AddInMemoryCollection(new Dictionary<string, string>
                    {
                        { "serverOptions:name", "TestServer" },
                        { "serverOptions:listeners:0:ip", "Any" },
                        { "serverOptions:listeners:0:port", DefaultServerPort.ToString() }
                    });
                })
                .AddServer<StringPackageInfo, CommandLinePipelineFilter>(builder =>
                {
                    builder
                    .UseCommand()
                    .UseSession<MySession>()
                    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                    .ConfigureContainer<ContainerBuilder>(builder =>
                    {
                        builder.RegisterType<ADD>().As<IAsyncCommand<ISession, StringPackageInfo>>();
                        builder.RegisterType<MULT>().As<IAsyncCommand<ISession, StringPackageInfo>>();
                        builder.RegisterType<SUB>().As<IAsyncCommand<ISession, StringPackageInfo>>();
                        builder.RegisterType<DIV>().As<IAsyncCommand<MySession, StringPackageInfo>>();
                    });
                }).BuildAsServer();

            Assert.Equal("TestServer", server.Name);

            Assert.True(await server.StartAsync());
            OutputHelper.WriteLine("Server started.");

            var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await client.ConnectAsync(GetDefaultServerEndPoint());
            OutputHelper.WriteLine("Connected.");

            using (var stream = new NetworkStream(client))
            using (var streamReader = new StreamReader(stream, Utf8Encoding, true))
            using (var streamWriter = new StreamWriter(stream, Utf8Encoding, 1024 * 1024 * 4))
            {
                await streamWriter.WriteAsync("ADD 1 2 3\r\n");
                await streamWriter.FlushAsync();
                var line = await streamReader.ReadLineAsync();
                Assert.Equal("6", line);

                await streamWriter.WriteAsync("MULT 2 5\r\n");
                await streamWriter.FlushAsync();
                line = await streamReader.ReadLineAsync();
                Assert.Equal("10", line);

                await streamWriter.WriteAsync("SUB 8 2\r\n");
                await streamWriter.FlushAsync();
                line = await streamReader.ReadLineAsync();
                Assert.Equal("6", line);

                await streamWriter.WriteAsync("DIV 8 2\r\n");
                await streamWriter.FlushAsync();
                line = await streamReader.ReadLineAsync();
                Assert.Equal("4", line);
            }

            await server.StopAsync();
        }
    }
}
