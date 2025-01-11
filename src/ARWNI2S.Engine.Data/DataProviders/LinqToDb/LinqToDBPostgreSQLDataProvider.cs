// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using LinqToDB;
using LinqToDB.Common;
using LinqToDB.Data;
using LinqToDB.DataProvider.PostgreSQL;
using System.Data.Common;

namespace ARWNI2S.Engine.Data.DataProviders.LinqToDb
{
    /// <summary>
    /// Represents a data provider for PostgreSQL
    /// </summary>
    public partial class LinqToDBPostgreSQLDataProvider : PostgreSQLDataProvider
    {
        public LinqToDBPostgreSQLDataProvider() : base(ProviderName.PostgreSQL, PostgreSQLVersion.v95) { }

        public override void SetParameter(DataConnection dataConnection, DbParameter parameter, string name, DbDataType dataType, object value)
        {
            if (value is string && dataType.DataType == DataType.NVarChar)
            {
                dataType = dataType.WithDbType("citext");
            }

            base.SetParameter(dataConnection, parameter, name, dataType, value);
        }
    }
}
