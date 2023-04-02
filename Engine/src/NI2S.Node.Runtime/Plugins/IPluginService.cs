using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NI2S.Node.Plugins
{
    internal interface IPluginService
    {
        Task InstallPluginsAsync();
        Task UpdatePluginsAsync();
    }
}
