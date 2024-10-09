namespace ARWNI2S.Node.Data.Entities.Users
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