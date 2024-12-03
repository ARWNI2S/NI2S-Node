using ARWNI2S.Data.Entities.Users;
using System.Globalization;

namespace ARWNI2S.Node
{
    /// <summary>
    /// Represents work context
    /// </summary>
    public interface IWorkContext
    {
        /// <summary>
        /// Gets the current user
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task<User> GetCurrentUserAsync();

        /// <summary>
        /// Sets the current user
        /// </summary>
        /// <param name="user">Current user</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task SetCurrentUserAsync(User user = null);

        /// <summary>
        /// Gets current user working cultureInfo
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task<CultureInfo> GetWorkingCultureAsync();

        /// <summary>
        /// Sets current user working cultureInfo
        /// </summary>
        /// <param name="cultureInfo">CultureInfo</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task SetWorkingCultureAsync(CultureInfo cultureInfo);

    }
}
