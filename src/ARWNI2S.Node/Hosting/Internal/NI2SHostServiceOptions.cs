using ARWNI2S.Engine.Builder;

namespace ARWNI2S.Node.Hosting.Internal
{
    internal sealed class NI2SHostServiceOptions
    {
        public Action<IEngineBuilder> ConfigureEngine { get; set; }

        public NI2SHostingOptions HostingOptions { get; set; } = default!; // Always set when options resolved by DI

        public AggregateException HostingStartupExceptions { get; set; }
    }
}
