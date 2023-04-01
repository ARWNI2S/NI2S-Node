using NI2S.Node.Hosting.Builder;
using System;

namespace NI2S.Node.Hosting
{
    internal sealed class GenericWebHostServiceOptions
    {
        public Action<IApplicationBuilder>? ConfigureApplication { get; set; }

        public WebHostOptions WebHostOptions { get; set; } = default!; // Always set when options resolved by DI

        public AggregateException? HostingStartupExceptions { get; set; }
    }
}