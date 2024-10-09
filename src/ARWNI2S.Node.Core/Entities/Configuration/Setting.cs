using ARWNI2S.Node.Core.Entities.Localization;

namespace ARWNI2S.Node.Core.Entities.Configuration
{
    /// <summary>
    /// Represents a setting
    /// </summary>
    public partial class Setting : BaseDataEntity, ILocalizedEntity
    {
        public Setting()
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="value">Value</param>
        /// <param name="serverId">Server identifier</param>
        public Setting(string name, string value, int serverId = 0)
        {
            Name = name;
            Value = value;
            ServerId = serverId;
        }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the server for which this setting is valid. 0 is set when the setting is for all servers
        /// </summary>
        public int ServerId { get; set; }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns>Result</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
