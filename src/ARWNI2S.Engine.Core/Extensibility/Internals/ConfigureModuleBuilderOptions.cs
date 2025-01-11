using ARWNI2S.Extensibility;
using Microsoft.Extensions.Options;

namespace ARWNI2S.Engine.Extensibility.Internals
{
    internal class ConfigureModuleBuilderOptions : IConfigureOptions<ModuleBuilderOptions>
    {
        private readonly ICollection<IModuleDataSource> _dataSources;

        public ConfigureModuleBuilderOptions(ICollection<IModuleDataSource> dataSources)
        {
            _dataSources = dataSources;
        }

        public void Configure(ModuleBuilderOptions options)
        {

        }
    }
}