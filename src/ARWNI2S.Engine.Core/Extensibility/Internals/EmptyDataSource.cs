// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Extensibility;
using Microsoft.Extensions.Primitives;

namespace ARWNI2S.Engine.Extensibility.Internals
{
    internal class EmptyDataSource : IModuleDataSource
    {
        public IReadOnlyList<IModule> Modules => [];

        public IChangeToken GetChangeToken()
        {
            return default;
        }

        public IReadOnlyList<IModule> GetGroupedModules(ModuleGroupContext context)
        {
            return [];
        }
    }
}
