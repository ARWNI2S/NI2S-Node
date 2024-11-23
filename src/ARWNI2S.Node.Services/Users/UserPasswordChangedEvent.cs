using ARWNI2S.Node.Data.Entities.Users;

namespace ARWNI2S.Node.Services.Users
{
    /// <summary>
    /// User password changed event
    /// </summary>
    public partial class UserPasswordChangedEvent
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="password">Password</param>
        public UserPasswordChangedEvent(UserPassword password)
        {
            Password = password;
        }

        /// <summary>
        /// User password
        /// </summary>
        public UserPassword Password { get; }
    }
}