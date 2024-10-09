﻿using ARWNI2S.Node.Data.Entities.Users;

namespace ARWNI2S.Node.Data.Services.Users
{
    /// <summary>
    /// User registration request
    /// </summary>
    public partial class UserRegistrationRequest
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="email">Email</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="passwordFormat">Password format</param>
        /// <param name="serverId">Server identifier</param>
        /// <param name="isApproved">Is approved</param>
        public UserRegistrationRequest(User user, string email, string username,
            string password,
            PasswordFormat passwordFormat,
            int serverId,
            bool isApproved = true)
        {
            User = user;
            Email = email;
            Username = username;
            Password = password;
            PasswordFormat = passwordFormat;
            ServerId = serverId;
            IsApproved = isApproved;
            //IsWeb3Registration = isWeb3Registration;
        }

        /// <summary>
        /// User
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Password format
        /// </summary>
        public PasswordFormat PasswordFormat { get; set; }

        /// <summary>
        /// Node identifier
        /// </summary>
        public int ServerId { get; set; }

        /// <summary>
        /// Is approved
        /// </summary>
        public bool IsApproved { get; set; }

    }
}
