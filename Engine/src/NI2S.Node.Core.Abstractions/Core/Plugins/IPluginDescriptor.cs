using System;
using System.Collections.Generic;
using System.Reflection;

namespace NI2S.Node.Core.Plugins
{
    public interface IPluginDescriptor : IDescriptor
    {
        /// <summary>
        /// Gets or sets the plugin group
        /// </summary>
        string Group { get; set; }

        /// <summary>
        /// Gets or sets the supported versions of nopCommerce
        /// </summary>
        IList<string> SupportedVersions { get; set; }

        /// <summary>
        /// Gets or sets the author
        /// </summary>
        string Author { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the name of the assembly file
        /// </summary>
        string AssemblyFileName { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the list of store identifiers in which this plugin is available. If empty, then this plugin is available in all stores
        /// </summary>
        IList<int> LimitedToStores { get; set; }

        /// <summary>
        /// Gets or sets the list of customer role identifiers for which this plugin is available. If empty, then this plugin is available for all ones.
        /// </summary>
        IList<int> LimitedToCustomerRoles { get; set; }

        /// <summary>
        /// Gets or sets the list of plugins' system name that this plugin depends on
        /// </summary>
        IList<string> DependsOn { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether plugin is installed
        /// </summary>
        bool Installed { get; set; }

        /// <summary>
        /// Gets or sets the plugin type
        /// </summary>
        Type PluginType { get; set; }

        /// <summary>
        /// Gets or sets the original assembly file
        /// </summary>
        string OriginalAssemblyFile { get; set; }

        /// <summary>
        /// Gets or sets the list of all library files in the plugin directory
        /// </summary>
        IList<string> PluginFiles { get; set; }

        /// <summary>
        /// Gets or sets the assembly that is active in the engine
        /// </summary>
        Assembly ReferencedAssembly { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether need to show the plugin on plugins page
        /// </summary>
        bool ShowInPluginsList { get; set; }
    }
}
