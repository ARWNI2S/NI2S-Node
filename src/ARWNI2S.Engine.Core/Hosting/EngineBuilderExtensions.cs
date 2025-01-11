using ARWNI2S.Engine.Builder;
using ARWNI2S.Environment;
using ARWNI2S.Extensibility;

namespace ARWNI2S.Engine.Hosting
{
    internal static class EngineBuilderExtensions
    {
        internal static void ConfigureEngine(this IEngineBuilder engine)
        {
            //find startup configurations provided by other assemblies
            var typeFinder = Singleton<ITypeFinder>.Instance;
            var startupConfigurations = typeFinder.FindClassesOfType<IConfigureEngine>();

            //create and sort instances of startup configurations
            var instances = startupConfigurations
                .Select(startup => (IConfigureEngine)Activator.CreateInstance(startup))
                .Where(startup => startup != null)
                .OrderBy(startup => startup.Order);

            //configure engine
            foreach (var instance in instances)
                instance.ConfigureEngine(engine);
        }
    }
}
