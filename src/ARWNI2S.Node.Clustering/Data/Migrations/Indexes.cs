namespace ARWNI2S.Clustering.Data.Migrations
{
    [NI2SMigration("2024/10/11 13:13:13:1313131", "ARWNI2S.Clustering.Data indexes", MigrationProcessType.Installation)]
    public class Indexes : AutoReversingMigration
    {
        #region Methods

        public override void Up()
        {
            Create.Index("IX_NI2SNode_NodeId").OnTable(nameof(NI2SNode))
                .OnColumn(nameof(NI2SNode.NodeId)).Unique()
                .WithOptions().NonClustered();

            Create.Index("IX_NodeMapping_EntityId_EntityName").OnTable(nameof(NodeMapping))
                .OnColumn(nameof(NodeMapping.EntityId)).Ascending()
                .OnColumn(nameof(NodeMapping.EntityName)).Ascending()
                .WithOptions().NonClustered();
        }

        #endregion
    }
}
