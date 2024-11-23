using ARWNI2S.Node.Data.Entities.Security;
using ARWNI2S.Node.Data.Entities.Users;
using ARWNI2S.Node.Data.Extensions;
using FluentMigrator.Builders.Create.Table;

namespace ARWNI2S.Node.Data.Mapping.Builders.Security
{
    /// <summary>
    /// Represents a ACL record entity builder
    /// </summary>
    public partial class AclRecordBuilder : DataEntityBuilder<AclRecord>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(AclRecord.EntityName)).AsString(400).NotNullable()
                .WithColumn(nameof(AclRecord.UserRoleId)).AsInt32().ForeignKey<UserRole>();
        }

        #endregion
    }
}