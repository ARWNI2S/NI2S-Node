﻿using FluentMigrator.Builders.Create.Table;

namespace ARWNI2S.Data.Mapping.Builders.Logging
{
    /// <summary>
    /// Represents a activity log entity builder
    /// </summary>
    public partial class ActivityLogBuilder : DataEntityBuilder<ActivityLog>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(ActivityLog.Comment)).AsString(int.MaxValue).NotNullable()
                .WithColumn(nameof(ActivityLog.IpAddress)).AsString(200).Nullable()
                .WithColumn(nameof(ActivityLog.EntityName)).AsString(400).Nullable()
                .WithColumn(nameof(ActivityLog.ActivityLogTypeId)).AsInt32().ForeignKey<ActivityLogType>()
                .WithColumn(nameof(ActivityLog.UserId)).AsInt32().ForeignKey<User>();
        }

        #endregion
    }
}