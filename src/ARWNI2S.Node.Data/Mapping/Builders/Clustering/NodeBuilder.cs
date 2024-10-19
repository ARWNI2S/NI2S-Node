using ARWNI2S.Node.Core.Entities.Clustering;
using FluentMigrator.Builders.Create.Table;

namespace ARWNI2S.Node.Data.Mapping.Builders.Clustering
{
    /// <summary>
    /// Represents a node entity builder
    /// </summary>
    public partial class NodeBuilder : ServerEntityBuilder<NI2SNode>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(NI2SNode.Name)).AsString(64).NotNullable()
                .WithColumn(nameof(NI2SNode.NodeId)).AsGuid().NotNullable()
                .WithColumn(nameof(NI2SNode.Metadata)).AsString(1000).Nullable()
                .WithColumn(nameof(NI2SNode.IpAddress)).AsString(100).Nullable()
                .WithColumn(nameof(NI2SNode.PublicPort)).AsString(100).Nullable()
                .WithColumn(nameof(NI2SNode.RelayPort)).AsString(100).Nullable()
                .WithColumn(nameof(NI2SNode.Hosts)).AsString(1000).Nullable()
                .WithColumn(nameof(NI2SNode.CurrentState)).AsInt32().Nullable();
        }

        #endregion
    }
}