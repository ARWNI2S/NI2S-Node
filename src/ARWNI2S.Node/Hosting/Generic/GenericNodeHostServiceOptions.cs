using ARWNI2S.Engine.Builder;
using ARWNI2S.Node.Configuration.Options;

namespace ARWNI2S.Node.Hosting.Generic
{
    internal sealed class GenericNodeHostServiceOptions
    {
        public Action<IEngineBuilder> ConfigureEngine { get; set; }

        public NodeHostOptions NodeHostOptions { get; set; } = default!; // Always set when options resolved by DI

        public AggregateException HostingStartupExceptions { get; set; }
    }
}