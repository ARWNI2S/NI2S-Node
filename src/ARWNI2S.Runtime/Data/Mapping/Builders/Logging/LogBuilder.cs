using ARWNI2S.Data.Entities.Logging;
using ARWNI2S.Data.Entities.Users;
using ARWNI2S.Data.Extensions;
using FluentMigrator.Builders.Create.Table;

namespace ARWNI2S.Data.Mapping.Builders.Logging
{
    /// <summary>
    /// Represents a log entity builder
    /// </summary>
    public partial class LogBuilder : DataEntityBuilder<Log>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(Log.ShortMessage)).AsString(int.MaxValue).NotNullable()
                .WithColumn(nameof(Log.IpAddress)).AsString(200).Nullable()
                .WithColumn(nameof(Log.UserId)).AsInt32().Nullable().ForeignKey<User>();
        }

        #endregion
    }
}