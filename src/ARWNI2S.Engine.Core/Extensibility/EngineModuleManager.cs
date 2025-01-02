using ARWNI2S.Diagnostics;
using ARWNI2S.Extensibility;
using Microsoft.Extensions.Logging;

namespace ARWNI2S.Engine.Extensibility
{
    internal class EngineModuleManager : IEngineModuleManager
    {
        private readonly ILogger _logger;

        public ModuleCollection Modules { get; }

        public EngineModuleManager(ILoggerFactory logger)
        {
            _logger = logger.CreateLogger("ARWNI2S.Engine.ModuleManager");
            Modules = new ModuleCollection();
        }

        public void Register(EngineModule module, bool dependencyCheck = true)
        {
            _logger.LogDebug($"Registering engine module: {module.SystemName}");
            if (module.ModuleDependencies.Count > 0 && dependencyCheck)
            {
                _logger.LogDebug($"Checking dependencies...");
                foreach (var dependency in module.ModuleDependencies)
                {
                    var found = Modules.Where(m => m.Value.SystemName == dependency).ToArray();

                    if (found == null || found.Length == 0)
                        throw new ModuleDependencyException(module.SystemName, dependency);
                    //_logger.LogError($"Module {module.SystemName} has a dependency on {dependency} which is not registered.");
                }
            }

            Modules[module.GetType()] = module;


            _logger.LogInformation($"Module {module.SystemName} registered engine module: ");
        }

        void IEngineModuleManager.Register(IEngineModule module, bool dependencyCheck) => Register((EngineModule)module, dependencyCheck);

        IModuleCollection IEngineModuleManager.Modules => Modules;
    }

    [Serializable]
    internal class ModuleDependencyException : NI2SException
    {
        public string SystemName { get; }
        public string Dependency { get; }

        public ModuleDependencyException()
        {
        }

        public ModuleDependencyException(string message) : base(message)
        {
        }

        public ModuleDependencyException(string systemName, string dependency)
        {
            SystemName = systemName;
            Dependency = dependency;
        }

        public ModuleDependencyException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
