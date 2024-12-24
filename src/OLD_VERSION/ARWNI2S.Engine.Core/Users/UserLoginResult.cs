namespace ARWNI2S.Engine.Users
{
    /// <summary>
    /// User registration result
    /// </summary>
    public partial class UserLoginResult
    {
        public UserLoginResult()
        {
            Errors = [];
        }

        /// <summary>
        /// Gets a value indicating whether request has been completed successfully
        /// </summary>
        public bool Success => !Errors.Any();

        /// <summary>
        /// Add error
        /// </summary>
        /// <param name="error">Error</param>
        public void AddError(string error)
        {
            Errors.Add(error);
        }

        /// <summary>
        /// Errors
        /// </summary>
        public IList<string> Errors { get; set; }

        /// <summary>
        /// State
        /// </summary>
        public string State { get; set; }
    }
}