using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NI2S.Node.Protocol;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.Hosting;
using NI2S.Node;
using NI2S.Node.WebSocket.Server;
using NI2S.Node.WebSocket;
using NI2S.Node.Hosting;

namespace NI2S.Node.Tests.WebSocket
{
    public abstract class WebSocketServerTestBase : TestClassBase
    {
        public WebSocketServerTestBase(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {

        }

        protected INodeHostBuilder<WebSocketPackage> CreateWebSocketServerBuilder(Func<INodeHostBuilder<WebSocketPackage>, INodeHostBuilder<WebSocketPackage>>? configurator = null, IHostConfigurator hostConfigurator = null)
        {
            INodeHostBuilder<WebSocketPackage> builder = WebSocketHostBuilder.Create();
            
            if (configurator != null)
                builder = configurator(builder);

            return Configure(builder, hostConfigurator) as INodeHostBuilder<WebSocketPackage>;
        }
    }
}
