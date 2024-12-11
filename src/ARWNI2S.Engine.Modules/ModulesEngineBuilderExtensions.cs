using ARWNI2S.Engine.Builder;

namespace ARWNI2S.Core
{
    public static class ModulesEngineBuilderExtensions
    {
        /// <summary>
        /// Configure middleware checking whether database is installed
        /// </summary>
        /// <param name="engine">Builder for configuring an application's request pipeline</param>
        public static void UseAutoInstaller(this IEngineBuilder engine)
        {
            //engine.UseMiddleware<InstallUrlMiddleware>();
        }

    }
}
