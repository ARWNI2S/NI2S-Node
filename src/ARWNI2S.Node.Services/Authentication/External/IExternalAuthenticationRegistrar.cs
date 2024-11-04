using Microsoft.AspNetCore.Authentication;

namespace ARWNI2S.Node.Services.Authentication.External
{
    /// <summary>
    /// Interface to register (configure) an external authentication service (module)
    /// </summary>
    public interface IExternalAuthenticationRegistrar
    {
        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="builder">Authentication builder</param>
        void Configure(AuthenticationBuilder builder);
    }
}
