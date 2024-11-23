using ARWNI2S.Node.Data.Entities.Clustering;
using FluentMigrator.Builders.Create.Table;

namespace ARWNI2S.Node.Data.Mapping.Builders.Clustering
{
    /// <summary>
    /// Represents a node entity builder
    /// </summary>
    public partial class ClusterNodeBuilder : DataEntityBuilder<ClusterNode>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(ClusterNode.Name)).AsString(64).NotNullable()
                .WithColumn(nameof(ClusterNode.NodeId)).AsGuid().NotNullable()
                .WithColumn(nameof(ClusterNode.Metadata)).AsString(1000).Nullable()
                .WithColumn(nameof(ClusterNode.IpAddress)).AsString(100).Nullable()
                .WithColumn(nameof(ClusterNode.PublicPort)).AsString(100).Nullable()
                .WithColumn(nameof(ClusterNode.RelayPort)).AsString(100).Nullable()
                .WithColumn(nameof(ClusterNode.Hosts)).AsString(1000).Nullable()
                .WithColumn(nameof(ClusterNode.CurrentState)).AsInt32().Nullable();
        }

        #endregion
    }
}