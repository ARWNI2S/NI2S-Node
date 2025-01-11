// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Runtime.Serialization;

namespace ARWNI2S.Engine.Data
{
    /// <summary>
    /// Represents data provider type enumeration
    /// </summary>
    public enum DataProviderType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember(Value = "")]
        Unknown,

        /// <summary>
        /// MS SQL Server
        /// </summary>
        [EnumMember(Value = "sqlserver")]
        SqlServer,

        /// <summary>
        /// MySQL
        /// </summary>
        [EnumMember(Value = "mysql")]
        MySql,

        /// <summary>
        /// PostgreSQL
        /// </summary>
        [EnumMember(Value = "postgresql")]
        PostgreSQL
    }
}