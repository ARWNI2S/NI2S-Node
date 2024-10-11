﻿using ARWNI2S.Node.Core.Services.Helpers;
using ARWNI2S.Node.Data.Entities.Users;

namespace ARWNI2S.Node.Data.Services.Users
{
    /// <summary>
    /// User report service
    /// </summary>
    public partial class UserReportService : IUserReportService
    {
        #region Fields

        private readonly IUserService _userService;
        private readonly IDateTimeHelper _dateTimeHelper;
        //private readonly IRepository<User> _userRepository;

        #endregion

        #region Ctor

        public UserReportService(IUserService userService,
            IDateTimeHelper dateTimeHelper
            //IRepository<User> userRepository
            )
        {
            _userService = userService;
            _dateTimeHelper = dateTimeHelper;
            //_userRepository = userRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a report of users registered in the last days
        /// </summary>
        /// <param name="days">Users registered in the last days</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the number of registered users
        /// </returns>
        public virtual async Task<int> GetRegisteredUsersReportAsync(int days)
        {
            var date = (await _dateTimeHelper.ConvertToUserTimeAsync(DateTime.Now)).AddDays(-days);

            var registeredUserRole = await _userService.GetUserRoleBySystemNameAsync(UserDefaults.RegisteredRoleName);
            if (registeredUserRole == null)
                return 0;

            return (await _userService.GetAllUsersAsync(
                date,
                userRoleIds: new[] { registeredUserRole.Id })).Count;
        }

        #endregion
    }
}