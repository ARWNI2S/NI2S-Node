using ARWNI2S.Node.Core.Entities.ScheduleTasks;
using FluentMigrator.Builders.Create.Table;

namespace ARWNI2S.Node.Data.Mapping.Builders.ScheduleTasks
{
    /// <summary>
    /// Represents a schedule task entity builder
    /// </summary>
    public partial class ScheduleTaskBuilder : ServerEntityBuilder<ScheduleTask>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(ScheduleTask.Name)).AsString(int.MaxValue).NotNullable()
                .WithColumn(nameof(ScheduleTask.Type)).AsString(int.MaxValue).NotNullable();
        }

        #endregion
    }
}