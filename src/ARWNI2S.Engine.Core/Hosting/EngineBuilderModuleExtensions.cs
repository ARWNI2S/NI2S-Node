using ARWNI2S.Engine.Builder;

namespace ARWNI2S.Engine.Hosting
{
    public static class EngineBuilderModuleExtensions
    {
        public static IEngineBuilder UseModules(this IEngineBuilder builder)
        {
            //builder.ConfigureServices((hostingContext, services) => //2
            //{
            //    services.AddSingleton<EngineLifecycleSubject>();
            //    services.AddSingleton<IEngineLifecycleSubject>(provider => provider.GetRequiredService<EngineLifecycleSubject>());
            //    services.AddSingleton<ILifecycleSubject>(provider => provider.GetRequiredService<EngineLifecycleSubject>());

            //});

            return builder;
        }
    }
}
