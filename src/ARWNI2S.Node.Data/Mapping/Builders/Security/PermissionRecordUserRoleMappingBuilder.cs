using ARWNI2S.Node.Core.Entities.Security;
using ARWNI2S.Node.Core.Entities.Users;
using ARWNI2S.Node.Data.Extensions;
using FluentMigrator.Builders.Create.Table;

namespace ARWNI2S.Node.Data.Mapping.Builders.Security
{
    /// <summary>
    /// Represents a permission record user role mapping entity builder
    /// </summary>
    public partial class PermissionRecordUserRoleMappingBuilder : ServerEntityBuilder<PermissionRecordUserRoleMapping>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(NameCompatibilityManager.GetColumnName(typeof(PermissionRecordUserRoleMapping), nameof(PermissionRecordUserRoleMapping.PermissionRecordId)))
                    .AsInt32().PrimaryKey().ForeignKey<PermissionRecord>()
                .WithColumn(NameCompatibilityManager.GetColumnName(typeof(PermissionRecordUserRoleMapping), nameof(PermissionRecordUserRoleMapping.UserRoleId)))
                    .AsInt32().PrimaryKey().ForeignKey<UserRole>();
        }

        #endregion
    }
}