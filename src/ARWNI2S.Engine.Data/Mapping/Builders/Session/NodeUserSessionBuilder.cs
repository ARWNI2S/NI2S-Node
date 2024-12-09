using FluentMigrator.Builders.Create.Table;

namespace ARWNI2S.Data.Mapping.Builders.Session
{
    /// <summary>
    /// Represents a ACL record entity builder
    /// </summary>
    public partial class NodeUserSessionBuilder : DataEntityBuilder<SessionRecord>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(SessionRecord.SessionId)).AsString(400).NotNullable()
                .WithColumn(nameof(SessionRecord.UserId)).AsInt32().ForeignKey<User>();
        }

        #endregion
    }
}
