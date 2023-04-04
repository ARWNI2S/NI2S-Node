// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using NI2S.Network;
using NI2S.Network.Command;
using System.Reflection;

namespace NI2S.Node.Network.Server
{
    public static class NodeServerExtensions
    {
        public static ISocketServerHostBuilder UseServerCommands<TPackageInfo>(this ISocketServerHostBuilder<TPackageInfo> builder)
            where TPackageInfo : class
        {
            return builder.UseCommand((commandOptions) =>
             {
                 // register all commands in assembly
                 commandOptions.AddCommandAssembly(typeof(CommandBase).GetTypeInfo().Assembly);
             });
        }
    }
}
