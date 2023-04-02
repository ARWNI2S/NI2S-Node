using System.Security.Claims;

namespace NI2S.Node.Dummy
{
    internal interface IDummyAuthenticationFeature
    {
        ClaimsPrincipal User { get; set; }
    }
}