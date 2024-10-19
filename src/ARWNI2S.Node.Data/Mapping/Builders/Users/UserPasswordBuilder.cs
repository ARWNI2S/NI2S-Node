using ARWNI2S.Node.Core.Entities.Users;
using ARWNI2S.Node.Data.Extensions;
using FluentMigrator.Builders.Create.Table;

namespace ARWNI2S.Node.Data.Mapping.Builders.Users
{
    /// <summary>
    /// Represents a user password entity builder
    /// </summary>
    public partial class UserPasswordBuilder : ServerEntityBuilder<UserPassword>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table.WithColumn(nameof(UserPassword.UserId)).AsInt32().ForeignKey<User>();
        }

        #endregion
    }
}