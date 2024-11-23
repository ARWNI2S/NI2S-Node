using ARWNI2S.Node.Data.Entities.Scheduling;
using FluentMigrator.Builders.Create.Table;

namespace ARWNI2S.Node.Data.Mapping.Builders.ScheduleTasks
{
    /// <summary>
    /// Represents a schedule task entity builder
    /// </summary>
    public partial class ScheduleTaskBuilder : DataEntityBuilder<ClusterJob>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(ClusterJob.Name)).AsString(int.MaxValue).NotNullable()
                .WithColumn(nameof(ClusterJob.Type)).AsString(int.MaxValue).NotNullable();
        }

        #endregion
    }
}