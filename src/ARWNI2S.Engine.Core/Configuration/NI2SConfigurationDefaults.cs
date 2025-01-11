// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.


using ARWNI2S.Configuration;

namespace ARWNI2S.Engine.Configuration
{
    internal static class NI2SConfigurationDefaults
    {
        internal static readonly string CommonConfigName = "Commons";
        internal static readonly string PluginConfigName = "Plugins";

        internal static string GetConfigName<TConfig>() where TConfig : class, IConfig
        {
            var name = typeof(TConfig).Name;
            if (name.EndsWith("Config"))
                name = name.Replace("Config", "");
            else if (name.EndsWith("Configuration"))
                name = name.Replace("Configuration", "");

            return name;
        }
    }
}
