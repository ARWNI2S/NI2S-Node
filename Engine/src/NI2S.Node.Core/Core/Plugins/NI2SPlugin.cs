// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.


using System.Reflection;
using System.Threading.Tasks;

namespace NI2S.Node.Core.Plugins
{
    public class NI2SPlugin : IPlugin
    {
        public Assembly Assembly { get; }
        public IDescriptor PluginDescriptor { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public NI2SPlugin(Assembly assembly)
        {
            Assembly = assembly;
        }

        public string GetConfigurationPageUrl()
        {
            throw new System.NotImplementedException();
        }

        public Task InstallAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task UninstallAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync(string currentVersion, string targetVersion)
        {
            throw new System.NotImplementedException();
        }

        public Task PreparePluginToUninstallAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
