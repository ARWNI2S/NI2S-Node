// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.


using System.Collections.Generic;
using System.Reflection;

namespace NI2S.Node.Core.Plugins
{
    public interface INI2SPluginFactory
    {
        IEnumerable<NI2SPlugin> GetPlugins(Assembly assembly);
    }
}