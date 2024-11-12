using ARWNI2S.Infrastructure.Entities;

namespace ARWNI2S.Node.Services.Plugins
{
    /// <summary>
    /// Represents a module manager
    /// </summary>
    /// <typeparam name="TModule">Type of module</typeparam>
    public partial interface IModuleManager<TModule> where TModule : class, IModule
    {
        /// <summary>
        /// Load all modules
        /// </summary>
        /// <param name="user">Filter by user; pass null to load all modules</param>
        /// <param name="nodeId">Filter by node; pass 0 to load all modules</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the list of modules
        /// </returns>
        Task<IList<TModule>> LoadAllModulesAsync(IUser user = null, int nodeId = 0);

        /// <summary>
        /// Load module by system name
        /// </summary>
        /// <param name="systemName">System name</param>
        /// <param name="user">Filter by user; pass null to load all modules</param>
        /// <param name="nodeId">Filter by node; pass 0 to load all modules</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the module
        /// </returns>
        Task<TModule> LoadModuleBySystemNameAsync(string systemName, IUser user = null, int nodeId = 0);

        /// <summary>
        /// Load active modules
        /// </summary>
        /// <param name="systemNames">System names of active modules</param>
        /// <param name="user">Filter by user; pass null to load all modules</param>
        /// <param name="nodeId">Filter by node; pass 0 to load all modules</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the list of active modules
        /// </returns>
        Task<IList<TModule>> LoadActiveModulesAsync(List<string> systemNames, IUser user = null, int nodeId = 0);

        /// <summary>
        /// Check whether the passed module is active
        /// </summary>
        /// <param name="module">Module to check</param>
        /// <param name="systemNames">System names of active modules</param>
        /// <returns>Result</returns>
        bool IsModuleActive(TModule module, List<string> systemNames);

        ///// <summary>
        ///// Get module logo URL
        ///// </summary>
        ///// <param name="module">Module</param>
        ///// <returns>
        ///// A task that represents the asynchronous operation
        ///// The task result contains the logo URL
        ///// </returns>
        //Task<string> GetModuleLogoUrlAsync(TModule module);
    }
}