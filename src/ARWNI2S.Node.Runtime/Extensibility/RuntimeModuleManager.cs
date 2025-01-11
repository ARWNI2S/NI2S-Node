// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Cluster.Extensibility;
using ARWNI2S.Engine.Extensibility;
using ARWNI2S.Extensibility;
using Microsoft.Extensions.Logging;

namespace ARWNI2S.Node.Extensibility
{
    internal class RuntimeModuleManager : IModuleManager
    {
        private readonly IEngineModuleManager _engineModuleManager;
        private readonly ILogger _logger;

        public IEnumerable<KeyValuePair<Type, IModule>> AllModules => GetAllModules();

        public ModuleCollection FrameworkModules { get; }
        IModuleCollection IModuleManager.FrameworkModules => FrameworkModules;

        public IModuleCollection EngineModules => _engineModuleManager.Modules;

        public ModuleCollection NodeModules { get; }
        IModuleCollection IModuleManager.NodeModules => NodeModules;

        public ModuleCollection UserModules { get; }
        IModuleCollection IModuleManager.UserModules => UserModules;

        public RuntimeModuleManager(IEngineModuleManager engineModuleManager, ILoggerFactory logger)
        {
            _logger = logger.CreateLogger("ARWNI2S.Runtime.ModuleManager");
            _engineModuleManager = engineModuleManager;

            FrameworkModules = new ModuleCollection();
            NodeModules = new ModuleCollection();
            UserModules = new ModuleCollection();
        }

        public void Register(IModule module)
        {
            if (module is FrameworkModule frameworkModule)
            {
                Register(frameworkModule);
            }
            else if (module is EngineModule engineModule)
            {
                Register(engineModule);
            }
            else if (module is ClusterModule clusterModule)
            {
                Register(clusterModule);
            }
            else
            {
                _logger.LogDebug("Registering user module: {SystemName}", module.SystemName);
                if (module.ModuleDependencies.Count > 0)
                {
                    _logger.LogDebug($"Checking dependencies...");
                    foreach (var dependency in module.ModuleDependencies)
                    {
                        var found = GetAllModules().Where(m => m.Value.SystemName == dependency).ToArray();

                        if (found == null || found.Length == 0)
                            throw new ModuleDependencyException(module.SystemName, dependency);
                        //_logger.LogError($"Module {module.SystemName} has a dependency on {dependency} which is not registered.");
                    }
                }

                UserModules[module.GetType()] = module;

                _logger.LogInformation("Module {SystemName} registered as user module.", module.SystemName);
            }
        }

        public void Register(EngineModule module)
        {
            if (module is FrameworkModule frameworkModule)
            {
                Register(frameworkModule);
            }
            else
            {
                _logger.LogDebug("Registering engine module: {SystemName}", module.SystemName);
                if (module.ModuleDependencies.Count > 0)
                {
                    _logger.LogDebug("Checking dependencies...");
                    foreach (var dependency in module.ModuleDependencies)
                    {
                        var found = GetAllModules().Where(m => m.Value.SystemName == dependency).ToArray();

                        if (found == null || found.Length == 0)
                            throw new ModuleDependencyException(module.SystemName, dependency);
                        //_logger.LogError($"Module {module.SystemName} has a dependency on {dependency} which is not registered.");
                    }
                }

                _engineModuleManager.Register(module, false);

                //_logger.LogInformation($"Module {module.SystemName} registered as engine module.");

            }
        }

        public void Register(FrameworkModule module)
        {
            _logger.LogDebug("Registering framework module: {SystemName}",module.SystemName);
            if (module.ModuleDependencies.Count > 0)
            {
                _logger.LogDebug("Checking dependencies...");
                foreach (var dependency in module.ModuleDependencies)
                {
                    var found = GetAllModules().Where(m => m.Value.SystemName == dependency).ToArray();

                    if (found == null || found.Length == 0)
                        throw new ModuleDependencyException(module.SystemName, dependency);
                    //_logger.LogError($"Module {module.SystemName} has a dependency on {dependency} which is not registered.");
                }
            }

            FrameworkModules[module.GetType()] = module;

            _logger.LogInformation("Module {SystemName} registered as framework module.", module.SystemName);
        }

        public void Register(ClusterModule module)
        {
            _logger.LogDebug("Registering framework module: {SystemName}", module.SystemName);
            if (module.ModuleDependencies.Count > 0)
            {
                _logger.LogDebug("Checking dependencies...");
                foreach (var dependency in module.ModuleDependencies)
                {
                    var found = GetAllModules().Where(m => m.Value.SystemName == dependency).ToArray();

                    if (found == null || found.Length == 0)
                        throw new ModuleDependencyException(module.SystemName, dependency);
                    //_logger.LogError($"Module {module.SystemName} has a dependency on {dependency} which is not registered.");
                }
            }

            NodeModules[module.GetType()] = module;

            _logger.LogInformation("Module {SystemName} registered as framework module.", module.SystemName);
        }

        private IEnumerable<KeyValuePair<Type, IModule>> GetAllModules()
        {
            return FrameworkModules.Concat(EngineModules).Concat(NodeModules).Concat(UserModules);
        }
    }
}
