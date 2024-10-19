using ARWNI2S.Node.Core.Entities.Security;

namespace ARWNI2S.Node.Services.Security
{
    /// <summary>
    /// Permission provider
    /// </summary>
    public interface IPermissionProvider
    {
        /// <summary>
        /// Get permissions
        /// </summary>
        /// <returns>Permissions</returns>
        IEnumerable<PermissionRecord> GetPermissions();

        /// <summary>
        /// Get default permissions
        /// </summary>
        /// <returns>Default permissions</returns>
        HashSet<(string systemRoleName, PermissionRecord[] permissions)> GetDefaultPermissions();
    }
}
