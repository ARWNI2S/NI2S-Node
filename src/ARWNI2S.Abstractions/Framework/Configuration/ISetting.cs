using ARWNI2S.Framework.Data;
using ARWNI2S.Framework.Localization;

namespace ARWNI2S.Framework.Configuration
{
    /// <summary>
    /// Represents a setting
    /// </summary>
    public interface ISetting : IDataEntity, ILocalizedEntity
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        string Value { get; set; }

        /// <summary>
        /// Gets or sets the node for which this setting is valid. 0 is set when the setting is for all nodes
        /// </summary>
        int NodeId { get; set; }
    }
}