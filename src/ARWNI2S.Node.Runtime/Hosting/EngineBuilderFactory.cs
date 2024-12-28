using ARWNI2S.Engine.Builder;
using ARWNI2S.Extensibility;

namespace ARWNI2S.Node.Hosting
{
    internal class EngineBuilderFactory : IEngineBuilderFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initialize a new factory instance with an <see cref="IServiceProvider" />.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> used to resolve dependencies and initialize components.</param>
        public EngineBuilderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Create an <see cref="IEngineBuilder" /> builder given a <paramref name="nodeModules" />.
        /// </summary>
        /// <param name="nodeModules">An <see cref="IModuleCollection"/> of NI2S modules.</param>
        /// <returns>An <see cref="IEngineBuilder"/> configured with <paramref name="nodeModules"/>.</returns>
        public IEngineBuilder CreateBuilder(IModuleCollection nodeModules)
        {
            return new EngineBuilder(_serviceProvider, nodeModules);
        }
    }
}