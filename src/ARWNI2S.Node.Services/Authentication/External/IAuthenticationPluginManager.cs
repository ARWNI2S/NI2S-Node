﻿using ARWNI2S.Node.Core.Entities.Users;
using ARWNI2S.Node.Services.Plugins;

namespace ARWNI2S.Node.Services.Authentication.External
{
    /// <summary>
    /// Represents an authentication plugin
    /// </summary>
    public partial interface IAuthenticationPluginManager : IPluginManager<IExternalAuthenticationMethod>
    {
        /// <summary>
        /// Load active authentication methods
        /// </summary>
        /// <param name="user">Filter by user; pass null to load all plugins</param>
        /// <param name="nodeId">Filter by server; pass 0 to load all plugins</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the list of active authentication methods
        /// </returns>
        Task<IList<IExternalAuthenticationMethod>> LoadActivePluginsAsync(User user = null, int nodeId = 0);

        /// <summary>
        /// Check whether the passed authentication method is active
        /// </summary>
        /// <param name="authenticationMethod">Authentication method to check</param>
        /// <returns>Result</returns>
        bool IsPluginActive(IExternalAuthenticationMethod authenticationMethod);

        /// <summary>
        /// Check whether the authentication method with the passed system name is active
        /// </summary>
        /// <param name="systemName">System name of authentication method to check</param>
        /// <param name="user">Filter by user; pass null to load all plugins</param>
        /// <param name="nodeId">Filter by server; pass 0 to load all plugins</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the result
        /// </returns>
        Task<bool> IsPluginActiveAsync(string systemName, User user = null, int nodeId = 0);
    }
}