using ARWNI2S.Node.Core.Entities.Directory;
using FluentMigrator.Builders.Create.Table;

namespace ARWNI2S.Node.Data.Mapping.Builders.Directory
{
    /// <summary>
    /// Represents a measure dimension entity builder
    /// </summary>
    public partial class MeasureTimeBuilder : ServerEntityBuilder<MeasureTime>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(MeasureTime.Name)).AsString(100).NotNullable()
                .WithColumn(nameof(MeasureTime.SystemKeyword)).AsString(100).NotNullable()
                .WithColumn(nameof(MeasureTime.Ratio)).AsDecimal(18, 8);
        }

        #endregion
    }
}