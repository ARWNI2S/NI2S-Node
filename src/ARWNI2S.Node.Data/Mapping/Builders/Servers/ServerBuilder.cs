namespace ARWNI2S.Node.Data.Mapping.Builders.Servers
{
    /// <summary>
    /// Represents a node entity builder
    /// </summary>
    public partial class ServerBuilder : ServerEntityBuilder<BladeServer>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(BladeServer.Name)).AsString(400).NotNullable()
                .WithColumn(nameof(BladeServer.Url)).AsString(400).NotNullable()
                .WithColumn(nameof(BladeServer.Hosts)).AsString(1000).Nullable()
                .WithColumn(nameof(BladeServer.CompanyName)).AsString(1000).Nullable()
                .WithColumn(nameof(BladeServer.CompanyAddress)).AsString(1000).Nullable()
                .WithColumn(nameof(BladeServer.CompanyPhoneNumber)).AsString(1000).Nullable()
                .WithColumn(nameof(BladeServer.CompanyVat)).AsString(1000).Nullable();
        }

        #endregion
    }
}