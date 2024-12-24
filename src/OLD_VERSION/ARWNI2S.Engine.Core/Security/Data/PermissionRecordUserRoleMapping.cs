using ARWNI2S.Data.Entities;

namespace ARWNI2S.Engine.Security.Data
{
    /// <summary>
    /// Represents a permission record-user role mapping class
    /// </summary>
    public partial class PermissionRecordUserRoleMapping : DataEntity
    {
        /// <summary>
        /// Gets or sets the permission record identifier
        /// </summary>
        public int PermissionRecordId { get; set; }

        /// <summary>
        /// Gets or sets the user role identifier
        /// </summary>
        public int UserRoleId { get; set; }
    }
}