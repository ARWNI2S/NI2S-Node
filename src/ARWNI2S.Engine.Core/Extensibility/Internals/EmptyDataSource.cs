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
