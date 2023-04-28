using System.Collections.Generic;
using System.Reflection;

namespace NI2S.Node.Engine
{
    public interface IModuleFactory
    {
        ICollection<IEngineModule> GetModules(Assembly assembly);
    }
}
