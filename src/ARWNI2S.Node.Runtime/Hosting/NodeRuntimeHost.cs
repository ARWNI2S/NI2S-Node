using SuperSocket.Server.Host;

namespace ARWNI2S.Runtime.Hosting
{
    internal sealed class NodeRuntimeHost
    {
        public static void ConfigureNodeRuntime(NodeHostBuilder builder)
        {
            builder.

            builder.ConfigureSuperSocket(options =>
            {
                options.EnableProxyProtocol = true;
            });
        }
    }
}
