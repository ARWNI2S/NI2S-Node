﻿namespace ARWNI2S.Node.Data.Entities.Users
{
    /// <summary>
    /// User activated event
    /// </summary>
    public partial class UserActivatedEvent
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="user">user</param>
        public UserActivatedEvent(User user)
        {
            User = user;
        }

        /// <summary>
        /// User
        /// </summary>
        public User User
        {
            get;
        }
    }
}
