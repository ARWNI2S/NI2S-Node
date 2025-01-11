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
