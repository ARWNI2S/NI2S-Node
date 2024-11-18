using ARWNI2S.Node.Core.Entities.Directory;
using ARWNI2S.Node.Data.Extensions;
using FluentMigrator.Builders.Create.Table;

namespace ARWNI2S.Node.Data.Mapping.Builders.Directory
{
    /// <summary>
    /// Represents a calendar measure time mapping entity builder
    /// </summary>
    public partial class CalendarMeasureTimeMappingBuilder : DataEntityBuilder<CalendarMeasureTimeMapping>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(NameCompatibilityManager.GetColumnName(typeof(CalendarMeasureTimeMapping), nameof(CalendarMeasureTimeMapping.CalendarId)))
                    .AsInt32().ForeignKey<Calendar>().PrimaryKey()
                .WithColumn(NameCompatibilityManager.GetColumnName(typeof(CalendarMeasureTimeMapping), nameof(CalendarMeasureTimeMapping.MeasureTimeId)))
                    .AsInt32().ForeignKey<MeasureTime>().PrimaryKey();
        }

        #endregion
    }
}
