// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.


using Microsoft.Extensions.Primitives;

namespace ARWNI2S.Extensibility
{
    public interface IModuleDataSource
    {
        IChangeToken GetChangeToken();
        IReadOnlyList<IModule> Modules { get; }
        IReadOnlyList<IModule> GetGroupedModules(ModuleGroupContext context);
    }
}
