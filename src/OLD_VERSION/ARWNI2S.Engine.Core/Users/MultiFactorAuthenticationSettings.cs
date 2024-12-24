using ARWNI2S.Engine.Configuration;

namespace ARWNI2S.Engine.Users
{
    /// <summary>
    /// Multi-factor authentication settings
    /// </summary>
    public partial class MultiFactorAuthenticationSettings : ISettings
    {
        #region Ctor

        public MultiFactorAuthenticationSettings()
        {
            ActiveAuthenticationMethodSystemNames = [];
        }

        #endregion

        /// <summary>
        /// Gets or sets system names of active multi-factor authentication methods
        /// </summary>
        public List<string> ActiveAuthenticationMethodSystemNames { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to force multi-factor authentication
        /// </summary>
        public bool ForceMultifactorAuthentication { get; set; }
    }
}
