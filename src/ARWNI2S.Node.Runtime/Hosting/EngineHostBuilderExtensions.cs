// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Extensibility;
using ARWNI2S.Hosting.Builder;
using ARWNI2S.Node.Extensibility;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Node.Hosting
{
    internal static class EngineHostBuilderExtensions
    {
        internal static INiisHostBuilder UseNI2SNodeIntegration(this INiisHostBuilder builder)
        {
            builder.ConfigureServices((hostingContext, services) => //4
            {
                services.AddSingleton<IModuleManager, RuntimeModuleManager>();


            });

            return builder;
        }
    }
}
