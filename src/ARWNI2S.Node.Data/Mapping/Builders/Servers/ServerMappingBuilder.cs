namespace ARWNI2S.Node.Data.Mapping.Builders.Servers
{
    /// <summary>
    /// Represents a server mapping entity builder
    /// </summary>
    public partial class ServerMappingBuilder : ServerEntityBuilder<ServerMapping>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(ServerMapping.EntityName)).AsString(400).NotNullable()
                .WithColumn(nameof(ServerMapping.ServerId)).AsInt32().ForeignKey<BladeServer>();
        }

        #endregion
    }
}