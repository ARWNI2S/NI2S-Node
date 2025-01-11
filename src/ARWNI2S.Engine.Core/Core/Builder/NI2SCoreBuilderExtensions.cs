// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Engine.Core.Builder;
using ARWNI2S.Engine.Plugins;
using Microsoft.Extensions.Configuration;

namespace ARWNI2S.Engine.Core.Builder
{
    internal static class NI2SCoreBuilderExtensions
    {
        internal static INiisCoreBuilder InitializePlugins(this INiisCoreBuilder builder, IConfiguration configuration)
        {
            var pluginConfig = new PluginConfig();
            configuration.GetSection(nameof(PluginConfig)).Bind(pluginConfig, options => options.BindNonPublicProperties = true);
            builder.PartManager.InitializePlugins(pluginConfig);
            return builder;
        }

    }
}
