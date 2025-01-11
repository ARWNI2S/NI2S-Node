// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Configuration;
using FluentMigrator.Runner.Initialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Configuration;

namespace ARWNI2S.Engine.Data.Configuration
{
    public partial class DataConfig : IConfig, IConnectionStringAccessor
    {
        /// <summary>
        /// Gets or sets a connection string
        /// </summary>
        [Secret]
        public string ConnectionString { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a data provider
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public DataProviderType DataProvider { get; set; } = DataProviderType.SqlServer;

        /// <summary>
        /// Gets or sets the wait time (in seconds) before terminating the attempt to execute a command and generating an error.
        /// By default, timeout isn't set and a default value for the current provider used.
        /// Set 0 to use infinite timeout.
        /// </summary>
        public int? SQLCommandTimeout { get; set; } = null;

        /// <summary>
        /// Gets or sets a value that indicates whether to add NoLock hint to SELECT statements (Reltates to SQL Server only)
        /// </summary>
        public bool WithNoLock { get; set; } = false;

        /// <summary>
        /// Gets a section name to load configuration
        /// </summary>
        [JsonIgnore]
        public string Name => nameof(ConfigurationManager.ConnectionStrings);

        /// <summary>
        /// Gets an order of configuration
        /// </summary>
        /// <returns>Order</returns>
        public int GetOrder() => 0; //display first
    }
}