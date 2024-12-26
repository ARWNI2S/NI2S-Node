using ARWNI2S.Engine.Configuration;
using ARWNI2S.Engine.Core.Builder;
using ARWNI2S.Engine.Plugins;
using Microsoft.Extensions.Configuration;

namespace ARWNI2S.Engine.Core.Builder
{
    public static class NI2SCoreBuilderExtensions
    {
        public static INiisCoreBuilder InitializePlugins(this INiisCoreBuilder builder, IConfiguration configuration)
        {
            var pluginConfig = new PluginConfig();
            configuration.GetSection(nameof(PluginConfig)).Bind(pluginConfig, options => options.BindNonPublicProperties = true);
            builder.PartManager.InitializePlugins(pluginConfig);
            return builder;
        }

    }
}
