// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using LinqToDB;

namespace ARWNI2S.Engine.Data
{
    /// <summary>
    /// Represents temporary storage
    /// </summary>
    /// <typeparam name="T">Storage record mapping class</typeparam>
    public partial class TempSqlDataStorage<T> : TempTable<T>, ITempDataStorage<T> where T : class
    {
        #region Ctor

        public TempSqlDataStorage(string storageName, IQueryable<T> query, IDataContext dataConnection)
            : base(dataConnection, storageName, query, tableOptions: TableOptions.NotSet | TableOptions.DropIfExists)
        {
            dataConnection.CloseAfterUse = true;
        }

        #endregion
    }
}