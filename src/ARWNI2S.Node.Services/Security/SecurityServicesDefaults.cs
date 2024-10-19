﻿using ARWNI2S.Node.Core.Caching;
using ARWNI2S.Node.Core.Entities.Security;

namespace ARWNI2S.Node.Services.Security
{
    /// <summary>
    /// Represents default values related to security services
    /// </summary>
    public static partial class SecurityServicesDefaults
    {
        #region Caching defaults

        #region Access control list

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : entity ID
        /// {1} : entity name
        /// </remarks>
        public static CacheKey AclRecordCacheKey => new("ni2s.aclrecord.{0}-{1}");

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : entity name
        /// </remarks>
        public static CacheKey EntityAclRecordExistsCacheKey => new("ni2s.aclrecord.exists.{0}");

        #endregion

        #region Permissions

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : permission system name
        /// {1} : user role ID
        /// </remarks>
        public static CacheKey PermissionAllowedCacheKey => new("ni2s.permissionrecord.allowed.{0}-{1}", PermissionAllowedPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        /// <remarks>
        /// {0} : permission system name
        /// </remarks>
        public static string PermissionAllowedPrefix => "ni2s.permissionrecord.allowed.{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : user role ID
        /// </remarks>
        public static CacheKey PermissionRecordsAllCacheKey => new("ni2s.permissionrecord.all.{0}", EntityCacheDefaults<PermissionRecord>.AllPrefix);

        #endregion

        #endregion
    }
}