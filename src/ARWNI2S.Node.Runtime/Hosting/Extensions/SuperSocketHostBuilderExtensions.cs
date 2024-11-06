using ARWNI2S.Node.Core.Configuration;
using ARWNI2S.Node.Core.Infrastructure;
using ARWNI2S.Node.Core.Network.Pipeline;
using ARWNI2S.Node.Core.Network.Protocol;
using ARWNI2S.Runtime.Network;
using SuperSocket.Server.Abstractions;
using SuperSocket.Server.Host;

namespace ARWNI2S.Runtime.Hosting.Extensions
{
    public static partial class SuperSocketHostBuilderExtensions
    {
        public static void ConfigureSuperSocketServer(this SuperSocketHostBuilder<NI2SProtoPacket> builder)
        {
            if (builder == null)
                return;

            builder.UseHostedService<NI2SFrontlineService>();

            builder.ConfigureSuperSocket(options =>
            {
                var ni2sSettings = EngineContext.Current.Resolve<NI2SSettings>();
                var nodeConfig = ni2sSettings.Get<NodeConfig>();

                options.EnableProxyProtocol = ni2sSettings.Get<HostingConfig>().UseProxy;
                options.Name = nodeConfig.NodeName;
                options.Listeners = [
                    new ListenOptions
                    {
                        Ip = "Any",
                        Port = nodeConfig.Port
                    },
                    new ListenOptions
                    {
                        Ip = "Any",
                        Port = nodeConfig.RelayPort
                    }
                    ];

                if (nodeConfig.EnableEditor)
                {

                }

            });

            builder.UsePipelineFilter<PipelineFilterRoot>();
            builder.UsePipelineFilterFactory<PipelineFilterFactory>();
        }
    }
}
