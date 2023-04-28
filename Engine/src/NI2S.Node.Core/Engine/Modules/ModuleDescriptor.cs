// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Newtonsoft.Json;
using NI2S.Node.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NI2S.Node.Engine
{
    /// <summary>
    /// Represents a module descriptor
    /// </summary>
    public partial class ModuleDescriptor : ModuleDescriptorBase, IDescriptor, IComparable<ModuleDescriptor>
    {
        #region Ctor

        public ModuleDescriptor()
        {
            SupportedVersions = new List<string>();
            DependsOn = new List<string>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get module descriptor from the description text
        /// </summary>
        /// <param name="text">Description text</param>
        /// <returns>Module descriptor</returns>
        public static ModuleDescriptor GetModuleDescriptorFromText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return new ModuleDescriptor();

            //get module descriptor from the JSON file
            var descriptor = JsonConvert.DeserializeObject<ModuleDescriptor>(text);

            if (!descriptor.SupportedVersions.Any())
                descriptor.SupportedVersions.Add("0.10");

            return descriptor;
        }

        /// <summary>
        /// Get the instance of the module
        /// </summary>
        /// <typeparam name="TModule">Type of the module</typeparam>
        /// <returns>Module instance</returns>
        public virtual TModule Instance<TModule>() where TModule : class, IEngineModule
        {
            //try to resolve module as unregistered service
            var instance = Core.Infrastructure.EngineContext.Current.ResolveUnregistered(ModuleType);

            //try to get typed instance
            var typedInstance = instance as TModule;
            if (typedInstance != null)
                typedInstance.ModuleDescriptor = this;

            return typedInstance;
        }

        /// <summary>
        /// Compares this instance with a specified ModuleDescriptor object
        /// </summary>
        /// <param name="other">The ModuleDescriptor to compare with this instance</param>
        /// <returns>An integer that indicates whether this instance precedes, follows, or appears in the same position in the sort order as the specified parameter</returns>
        public int CompareTo(ModuleDescriptor other)
        {
            if (DisplayOrder != other.DisplayOrder)
                return DisplayOrder.CompareTo(other.DisplayOrder);

            return string.Compare(SystemName, other.SystemName, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Returns the module as a string
        /// </summary>
        /// <returns>Value of the FriendlyName</returns>
        public override string ToString()
        {
            return FriendlyName;
        }

        /// <summary>
        /// Save module descriptor to the module description file
        /// </summary>
        public virtual void Save()
        {
            //since modules are loaded before IoC initialization using the default provider,
            //in order to avoid possible problems we use CommonHelper.DefaultFileProvider
            //instead of the main file provider
            var fileProvider = CommonHelper.DefaultFileProvider;

            //get the description file path
            if (OriginalAssemblyFile == null)
                throw new Exception($"Cannot load original assembly path for {SystemName} module.");

            var filePath = fileProvider.Combine(fileProvider.GetDirectoryName(OriginalAssemblyFile), EngineModuleDefaults.DescriptionFileName);
            if (!fileProvider.FileExists(filePath))
                throw new Exception($"Description file for {SystemName} module does not exist. {filePath}");

            //save the file
            var text = JsonConvert.SerializeObject(this, Formatting.Indented);
            fileProvider.WriteAllText(filePath, text, Encoding.UTF8);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the module group
        /// </summary>
        [JsonProperty(PropertyName = "Group")]
        public virtual string Group { get; set; }

        /// <summary>
        /// Gets or sets the module friendly name
        /// </summary>
        [JsonProperty(PropertyName = "FriendlyName")]
        public virtual string FriendlyName { get; set; }

        /// <summary>
        /// Gets or sets the supported versions of nopCommerce
        /// </summary>
        [JsonProperty(PropertyName = "SupportedVersions")]
        public virtual IList<string> SupportedVersions { get; set; }

        /// <summary>
        /// Gets or sets the author
        /// </summary>
        [JsonProperty(PropertyName = "Author")]
        public virtual string Author { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        [JsonProperty(PropertyName = "DisplayOrder")]
        public virtual int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the name of the assembly file
        /// </summary>
        [JsonProperty(PropertyName = "FileName")]
        public virtual string AssemblyFileName { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        [JsonProperty(PropertyName = "Description")]
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets the list of modules' system name that this module depends on
        /// </summary>
        [JsonProperty(PropertyName = "DependsOnSystemNames")]
        public virtual IList<string> DependsOn { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether module is installed
        /// </summary>
        [JsonIgnore]
        public virtual bool Installed { get; set; }

        /// <summary>
        /// Gets or sets the module type
        /// </summary>
        [JsonIgnore]
        public virtual Type ModuleType { get; set; }

        /// <summary>
        /// Gets or sets the original assembly file
        /// </summary>
        [JsonIgnore]
        public virtual string OriginalAssemblyFile { get; set; }

        /// <summary>
        /// Gets or sets the list of all library files in the module directory
        /// </summary>
        [JsonIgnore]
        public virtual IList<string> ModuleFiles { get; set; }

        /// <summary>
        /// Gets or sets the assembly that is active in the engine
        /// </summary>
        [JsonIgnore]
        public virtual Assembly ReferencedAssembly { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether need to show the module on modules page
        /// </summary>
        [JsonIgnore]
        public virtual bool ShowInModulesList { get; set; } = true;

        #endregion
    }
}
