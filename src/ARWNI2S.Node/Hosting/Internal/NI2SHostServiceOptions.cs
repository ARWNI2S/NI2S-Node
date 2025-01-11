// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

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
