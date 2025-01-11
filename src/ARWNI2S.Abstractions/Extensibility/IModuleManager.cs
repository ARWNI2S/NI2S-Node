// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Extensibility
{
    internal interface IModuleManager
    {
        IModuleCollection EngineModules { get; }
        IModuleCollection FrameworkModules { get; }
        IModuleCollection UserModules { get; }
        IModuleCollection NodeModules { get; }

        void Register(IModule module);
    }
}