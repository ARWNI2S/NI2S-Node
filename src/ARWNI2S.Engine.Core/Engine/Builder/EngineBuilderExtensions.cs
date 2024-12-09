using ARWNI2S.Engine.Builder;

namespace ARWNI2S.Core.Engine.Builder
{
    public static class EngineBuilderExtensions
    {
        /// <summary>
        /// Configure the engine framework
        /// </summary>
        /// <param name="engine">Builder for configuring the engine's framework</param>
        public static void ConfigureEngineFramework(this IEngineBuilder engine)
        {
            NI2SContext.Current.ConfigureEngine(engine);
        }
    }
}
