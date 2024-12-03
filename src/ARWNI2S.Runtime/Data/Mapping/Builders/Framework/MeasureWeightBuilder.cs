using ARWNI2S.Data.Entities.Framework;
using FluentMigrator.Builders.Create.Table;

namespace ARWNI2S.Data.Mapping.Builders.Framework
{
    /// <summary>
    /// Represents a measure weight entity builder
    /// </summary>
    public partial class MeasureWeightBuilder : DataEntityBuilder<MeasureWeight>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(MeasureWeight.Name)).AsString(100).NotNullable()
                .WithColumn(nameof(MeasureWeight.SystemKeyword)).AsString(100).NotNullable()
                .WithColumn(nameof(MeasureWeight.Ratio)).AsDecimal(18, 8);
        }

        #endregion
    }
}