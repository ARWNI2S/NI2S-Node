using ARWNI2S.Node.Core.Entities.Users;
using ARWNI2S.Node.Data.Extensions;
using FluentMigrator.Builders.Create.Table;

namespace ARWNI2S.Node.Data.Mapping.Builders.Users
{
    /// <summary>
    /// Represents a user user role mapping entity builder
    /// </summary>
    public partial class UserUserRoleMappingBuilder : DataEntityBuilder<UserUserRoleMapping>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(NameCompatibilityManager.GetColumnName(typeof(UserUserRoleMapping), nameof(UserUserRoleMapping.UserId)))
                    .AsInt32().ForeignKey<User>().PrimaryKey()
                .WithColumn(NameCompatibilityManager.GetColumnName(typeof(UserUserRoleMapping), nameof(UserUserRoleMapping.UserRoleId)))
                    .AsInt32().ForeignKey<UserRole>().PrimaryKey();
        }

        #endregion
    }
}