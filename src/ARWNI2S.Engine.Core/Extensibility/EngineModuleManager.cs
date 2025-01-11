// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

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
            _logger.LogDebug("Registering engine module: {SystemName}", module.SystemName);
            if (module.ModuleDependencies.Count > 0 && dependencyCheck)
            {
                _logger.LogDebug("Checking dependencies...");
                foreach (var dependency in module.ModuleDependencies)
                {
                    var found = Modules.Where(m => m.Value.SystemName == dependency).ToArray();

                    if (found == null || found.Length == 0)
                    {
                        _logger.LogError("Module {SystemName} has a dependency on {dependency} which is not registered.", module.SystemName, dependency);
                        throw new ModuleDependencyException(module.SystemName, dependency);
                    }
                }
            }

            Modules[module.GetType()] = module;


            _logger.LogInformation("Module {SystemName} registered engine module: ", module.SystemName);
        }

        void IEngineModuleManager.Register(IEngineModule module, bool dependencyCheck) => Register((EngineModule)module, dependencyCheck);

        IModuleCollection IEngineModuleManager.Modules => Modules;
    }

    [Serializable]
    internal class ModuleDependencyException : NI2SException
    {
        public string SystemName { get; }
        public string Dependency { get; }

        public ModuleDependencyException(string systemName, string dependency) : base($"Module {systemName} has a dependency on {dependency} which is not registered.")
        {
            SystemName = systemName;
            Dependency = dependency;
        }
    }
}
