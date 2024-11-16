using ARWNI2S.Infrastructure.Engine.Builder;

namespace ARWNI2S.Node.Hosting.Configuration.Options
{
    internal sealed class GenericNodeHostServiceOptions
    {
        public Action<IEngineBuilder> ConfigureEngine { get; set; }

        public NodeHostOptions NodeHostOptions { get; set; } = default!; // Always set when options resolved by DI

        public AggregateException HostingStartupExceptions { get; set; }
    }
}