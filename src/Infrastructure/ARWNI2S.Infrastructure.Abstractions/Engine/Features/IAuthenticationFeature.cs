using System.Security.Claims;

namespace ARWNI2S.Infrastructure.Engine.Features
{
    /// <summary>
    /// The authentication feature.
    /// </summary>
    public interface IAuthenticationFeature
    {
        /// <summary>
        /// Gets or sets the <see cref="ClaimsPrincipal"/> associated with the context.
        /// </summary>
        ClaimsPrincipal User { get; set; }
    }
}