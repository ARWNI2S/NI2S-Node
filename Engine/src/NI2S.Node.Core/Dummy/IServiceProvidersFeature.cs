using System;

namespace NI2S.Node.Dummy
{
    internal interface IServiceProvidersFeature
    {
        IServiceProvider RequestServices { get; set; }
    }
}