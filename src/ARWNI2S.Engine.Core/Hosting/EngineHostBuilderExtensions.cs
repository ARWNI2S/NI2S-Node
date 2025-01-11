﻿using ARWNI2S.Engine.Extensibility;
using ARWNI2S.Engine.Lifecycle;
using ARWNI2S.Extensibility;
using ARWNI2S.Hosting.Builder;
using Microsoft.Extensions.DependencyInjection;
using Orleans;

namespace ARWNI2S.Engine.Hosting
{
    internal static class EngineHostBuilderExtensions
    {
        internal static INiisHostBuilder UseNI2SEngine(this INiisHostBuilder builder)
        {
            builder.ConfigureServices((hostingContext, services) => //2
            {
                services.AddSingleton<IEngineModuleManager, EngineModuleManager>();

                services.AddSingleton<EngineLifecycleSubject>();
                services.AddSingleton<IEngineLifecycleSubject>(provider => provider.GetRequiredService<EngineLifecycleSubject>());
                services.AddSingleton<ILifecycleSubject>(provider => provider.GetRequiredService<EngineLifecycleSubject>());

            });

            return builder;
        }
    }
}
