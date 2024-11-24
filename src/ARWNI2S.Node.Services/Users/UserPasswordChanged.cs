using ARWNI2S.Node.Data.Entities.Users;

namespace ARWNI2S.Node.Services.Users
{
    /// <summary>
    /// User password changed notification
    /// </summary>
    public partial class UserPasswordChanged
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="password">Password</param>
        public UserPasswordChanged(UserPassword password)
        {
            Password = password;
        }

        /// <summary>
        /// User password
        /// </summary>
        public UserPassword Password { get; }
    }
}