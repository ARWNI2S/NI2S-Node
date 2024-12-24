using ARWNI2S.Hosting.Configuration;

namespace ARWNI2S.Hosting.Node
{
    internal sealed class NI2SHostServiceOptions
    {
        public Action<IEngineBuilder> ConfigureEngine { get; set; }

        public NI2SHostOptions NI2SHostOptions { get; set; } = default!; // Always set when options resolved by DI

        public AggregateException HostingStartupExceptions { get; set; }
    }
}