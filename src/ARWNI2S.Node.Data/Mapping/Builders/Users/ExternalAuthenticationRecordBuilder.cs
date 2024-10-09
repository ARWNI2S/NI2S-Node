namespace ARWNI2S.Node.Data.Mapping.Builders.Users
{
    /// <summary>
    /// Represents a external authentication record entity builder
    /// </summary>
    public partial class ExternalAuthenticationRecordBuilder : ServerEntityBuilder<ExternalAuthenticationRecord>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table.WithColumn(nameof(ExternalAuthenticationRecord.UserId)).AsInt32().ForeignKey<User>();
        }

        #endregion
    }
}