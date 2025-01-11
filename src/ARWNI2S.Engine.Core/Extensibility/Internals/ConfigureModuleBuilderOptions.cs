// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

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