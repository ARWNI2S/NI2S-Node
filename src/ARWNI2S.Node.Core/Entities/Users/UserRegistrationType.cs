namespace ARWNI2S.Node.Core.Entities.Users
{
    /// <summary>
    /// Represents the user registration type formatting enumeration
    /// </summary>
    public enum UserRegistrationType
    {
        /// <summary>
        /// Standard account creation
        /// </summary>
        Standard = 1,

        /// <summary>
        /// Validation is required after registration
        /// </summary>
        Validation = 2,

        /// <summary>
        /// A user should be approved by administrator
        /// </summary>
        AdminApproval = 3,

        /// <summary>
        /// Registration is disabled
        /// </summary>
        Disabled = 4
    }
}
