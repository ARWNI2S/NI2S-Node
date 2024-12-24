using System.Globalization;
using System.Security.Claims;

namespace ARWNI2S.Engine
{
    public interface IWorkContext
    {
        Task<ClaimsPrincipal> GetCurrentPrincipalAsync();
        Task<ClaimsIdentity> GetCurrentIdentityAsync();
        Task<CultureInfo> GetWorkingCultureAsync();
    }
}
