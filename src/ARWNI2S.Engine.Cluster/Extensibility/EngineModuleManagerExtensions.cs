using ARWNI2S.Extensibility;

namespace ARWNI2S.Engine.Cluster.Extensibility
{
    internal static class EngineModuleManagerExtensions
    {
        public static IModuleCollection GetLocallyInstalledModules(this IEngineModuleManager moduleManager)
        {
            //TODO
            return moduleManager.Modules;
        }
    }
}
