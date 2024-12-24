using ARWNI2S.Engine.Users.Data;

namespace ARWNI2S.Engine.Users.Events
{
    /// <summary>
    /// User registered event
    /// </summary>
    public partial class UserRegisteredEvent
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="user">user</param>
        public UserRegisteredEvent(User user)
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