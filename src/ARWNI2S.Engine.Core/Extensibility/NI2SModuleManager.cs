using ARWNI2S.Engine.Builder;
using ARWNI2S.Extensibility;
using Microsoft.Extensions.Logging;

namespace ARWNI2S.Engine.Extensibility
{
    internal class NI2SModuleManager : IEngineModuleManager
    {
        private readonly ILogger<NI2SModuleManager> _logger;

        ModuleCollection Modules { get; }

        public NI2SModuleManager(ILogger<NI2SModuleManager> logger)
        {
            _logger = logger;
            Modules = new ModuleCollection();
        }

        public void Register(EngineModule module)
        {
            _logger.LogDebug($"Registering module: {module.GetType()}");
            Modules[module.GetType()] = module;
        }

        void IEngineModuleManager.Register(IEngineModule module) => Register((EngineModule)module);

        IModuleCollection IEngineModuleManager.Modules => Modules;
    }
}
