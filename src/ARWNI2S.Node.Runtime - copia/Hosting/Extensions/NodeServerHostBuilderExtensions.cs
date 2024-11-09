using ARWNI2S.Engine.Network;
using ARWNI2S.Engine.Network.Command;
using ARWNI2S.Engine.Network.Protocol.Filters;
using ARWNI2S.Engine.Network.Session;
using ARWNI2S.Node.Core.Configuration;
using ARWNI2S.Node.Core.Infrastructure;
using ARWNI2S.Runtime.Console.Commands;
using System.Text;

namespace ARWNI2S.Runtime.Hosting.Extensions
{
    internal static class NodeHostBuilderExtensions
    {
        internal static void ConfigureNodeServerHost(this NodeHostBuilder builder)
        {
            builder.ConfigureNodeServer(options =>
            {
                var settings = Singleton<NI2SSettings>.Instance;
                var nodeConfig = settings.Get<NodeConfig>();

                options.ClearIdleSessionInterval = 120;
                options.DefaultTextEncoding = Encoding.UTF8;
                options.EnableProxyProtocol = settings.Get<HostingConfig>().UseProxy;
                options.IdleSessionTimeOut = 300;
                options.Listeners = [
                    new(){ Ip = "Any", Port = nodeConfig.Port },
                    new(){ Ip = "Any", Port = nodeConfig.RelayPort, NoDelay = true, UdpExclusiveAddressUse = true }
                    ];
                options.MaxPackageLength = 1024 * 1024;
                options.Name = nodeConfig.NodeName;
                options.PackageHandlingTimeOut = 30;
                options.ReadAsDemand = true;
                options.ReceiveBufferSize = 1024 * 4;
                options.ReceiveTimeout = 30;
                options.SendBufferSize = 1024 * 4;
                options.SendTimeout = 30;
            })
                .UsePipelineFilter<CommandLinePipelineFilter>()
                .UseCommand((commandOptions) =>
                {
                    // register commands one by one
                    commandOptions.AddCommand<InstallNodeCommand>();
                    //commandOptions.AddCommand<MULT>();
                    //commandOptions.AddCommand<SUB>();

                    // register all commands in one aassembly
                    //commandOptions.AddCommandAssembly(typeof(SUB).GetTypeInfo().Assembly);
                })
                .UseSession<NodeSession>()
                .UseInProcSessionContainer();

            builder.UseClearIdleSession();

            //builder.ConfigureNodeServerServer();
            //builder.UsePipelineFilterFactory();

        }

    }
}
