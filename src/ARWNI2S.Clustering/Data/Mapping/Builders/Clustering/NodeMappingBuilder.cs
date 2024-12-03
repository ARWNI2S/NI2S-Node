using ARWNI2S.Data.Entities.Clustering;
using ARWNI2S.Data.Extensions;
using FluentMigrator.Builders.Create.Table;

namespace ARWNI2S.Data.Mapping.Builders.Clustering
{
    /// <summary>
    /// Represents a server mapping entity builder
    /// </summary>
    public partial class NodeMappingBuilder : DataEntityBuilder<NodeMapping>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(NodeMapping.EntityName)).AsString(400).NotNullable()
                .WithColumn(nameof(NodeMapping.NodeId)).AsInt32().ForeignKey<ClusterNode>();
        }

        #endregion
    }
}