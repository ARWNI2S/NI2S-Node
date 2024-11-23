namespace ARWNI2S.Node.Data.Mapping.Builders.Users
{
    /// <summary>
    /// Represents a user attribute entity builder
    /// </summary>
    public partial class UserAttributeBuilder : ServerEntityBuilder<UserAttribute>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table.WithColumn(nameof(UserAttribute.Name)).AsString(400).NotNullable();
        }

        #endregion
    }
}